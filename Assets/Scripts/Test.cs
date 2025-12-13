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

public class Test : Btn
{
    [SerializeField] Text msg;
    bool canClick;
    void Awake()
    {
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
        Show(true);//Hide the button
        GetResult(img.EncodeToJPG());
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    void GetResult(byte[] img)
    {
        WWWForm form = new WWWForm();
        if (!Session.session.isGuest)
        {
            print($"token: {Session.session.Token}");
            form.AddField("token", Session.session.Token);
        }
        form.AddBinaryData("img", img);
        print(form);
        StartCoroutine(APIManager.Post("predict", form, (jsonText) =>
        {
            ColorJson colorJson = JsonUtility.FromJson<ColorJson>(jsonText);
            if (string.IsNullOrEmpty(colorJson.cname))
            {
                print("에러");
                msg.text = colorJson.color_id;
                Text text = btn.transform.GetChild(0).GetComponent<Text>();
                text.text = "퍼스널컬러 구하기";
                ColorBlock color = btn.colors;
                color.normalColor = new Color(1, 1, 1, 1);
                btn.colors = color;
                canClick = true;
                Show(true);
            }
            else
            {
                print("통과");
                Session.session.Predict(colorJson);
                SceneManager.LoadScene((int)SceneIndex.Result);
            }
        }));
    }

    void Show(bool value)
    {
        msg.text = "";
        Text text = btn.transform.GetChild(0).GetComponent<Text>();
        ColorBlock color = btn.colors;
        color.normalColor = new Color(1, 1, 1, value ? 1 : 0);
        btn.colors = color;
        text.text = value ? "예축하는중" : "";
    }
}
