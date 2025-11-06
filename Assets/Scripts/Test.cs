using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Button btn;
    [SerializeField] Text msg;
    bool canClick;
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {

            Permission.RequestUserPermission(Permission.Camera);
        }


        canClick = true;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            if (canClick)
                StartCoroutine(Capture());
        });
    }

    IEnumerator Capture()
    {
        canClick = false;
        SetBtn = false;//Hide the button
        yield return new WaitForEndOfFrame();
        var img = ScreenCapture.CaptureScreenshotAsTexture();
        StartCoroutine(Post(img.EncodeToJPG()));
        SetBtn = true;//Show the button
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
    IEnumerator Post(byte[] img)
    {
        WWWForm form = new WWWForm();
        if (!Session.session.isGeust){
            print($"token: {Session.session.Token}");
            form.AddField("token", Session.session.Token);
        }
        form.AddBinaryData("img", img);
        print(form);
        using (UnityWebRequest www = UnityWebRequest.Post(Env.Api("predict"), form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                ColorJson colorJson = JsonUtility.FromJson<ColorJson>(www.downloadHandler.text);
                if (string.IsNullOrEmpty(colorJson.description))
                {
                    print("에러");
                    msg.text = colorJson.color_id;
                    Text text = btn.transform.GetChild(0).GetComponent<Text>();
                    text.text = "퍼스널컬러 구하기";
                    ColorBlock color = btn.colors;
                    color.normalColor = new Color(1, 1, 1, 1);
                    btn.colors = color;
                    canClick = true;
                }
                else
                {
                    print("통과");
                    Session.session.Predict(colorJson);
                    SceneManager.LoadScene((int)Scene.Result);
                }
            }
        }
    }
    bool SetBtn
    {
        set
        {
            msg.text = "";
            Text text = btn.transform.GetChild(0).GetComponent<Text>();
            ColorBlock color = btn.colors;
            color.normalColor = new Color(1, 1, 1, value ? 1 : 0);
            btn.colors = color;
            text.text = value ? "예축하는중" : "";
        }
    }
}
