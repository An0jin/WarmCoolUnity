using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Toneiverse.DTO;
using Toneiverse;

public class Test : MSGBtn
{
    bool canClick;
    [SerializeField] BackBtn backBtn;
    Text btnText;
    void Awake()
    {
        btnText = GetComponentInChildren<Text>();
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {

            Permission.RequestUserPermission(Permission.Camera);
        }


        canClick = true;
        base.Awake();
    }

    protected override void OnClick()
    {
        if (canClick)
            StartCoroutine(Capture());
    }


    IEnumerator Capture()
    {
        canClick = false;
        Show(false);//Hide the button
        yield return new WaitForEndOfFrame();
        var img = ScreenCapture.CaptureScreenshotAsTexture();
        Show(true, "예측하는 중");//Hide the button
        GetResult(img.EncodeToJPG());
    }
    void GetResult(byte[] img)
    {
        WWWForm form = new WWWForm();
        form.AddField("token", Session.session.Token);
        form.AddBinaryData("img", img);
        StartCoroutine(APIManager.Post("predict", form, (jsonText) =>
        {
            ColorJson colorJson = JsonUtility.FromJson<ColorJson>(jsonText);
            if (string.IsNullOrEmpty(colorJson.cname))
            {
                Show(true, "퍼스널컬러 구하기");
                Error(colorJson.color_id);
                canClick = true;
            }
            else
            {
                Session.session.Predict(colorJson);
                SceneManager.LoadScene((int)SceneIndex.Result);
            }
        }));
    }

    void Show(bool value, string result = "")
    {
        msg.text = "";
        SetBtn(btn, value);
        SetBtn(backBtn.GetComponent<Button>(), value);
        btnText.text = value ? result : "";
    }
    void SetBtn(Button btn, bool value)
    {
        ColorBlock color = btn.colors;
        color.normalColor = new Color(1, 1, 1, value ? 1 : 0);
        btn.colors = color;
    }

}
