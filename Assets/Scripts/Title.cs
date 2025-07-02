using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class Title : MonoBehaviour
{
    InputField id, pw;
    Button SignIn,geust;
    bool isSignIn;
    Text msg;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Test());
        isSignIn = true;
        id = GameObject.Find("id").GetComponent<InputField>();
        pw = GameObject.Find("pw").GetComponent<InputField>();
        SignIn = GameObject.Find("SignIn").GetComponent<Button>();
        geust = GameObject.Find("Geust").GetComponent<Button>();
        msg = GameObject.Find("msg").GetComponent<Text>();
        SignIn.onClick.AddListener(() =>
        {
            if (isSignIn)
                StartCoroutine(LogIn());
        });
        geust.onClick.AddListener(() =>
        {
            Session.session.isGeust=true;
            SceneManager.LoadScene(2);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }
    IEnumerator LogIn()
    {
        print("Button pressed");
        isSignIn = false;
        msg.color = new Color(1, 1, 1);
        msg.text = "Logging in...";
        if (id.text == "" || pw.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "Please enter ID and password";
            isSignIn = true;
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", id.text);
        form.AddField("pw", pw.text);
        using (UnityWebRequest www = UnityWebRequest.Post(Env.Api("login"), form))
        {
            yield return www.SendWebRequest();
            Debug.Log("서버 응답 코드: " + www.responseCode);
            Debug.Log("서버 응답 내용: " + www.downloadHandler.text);
            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    InfoJson json = JsonUtility.FromJson<InfoJson>(www.downloadHandler.text);
                    if (json.msg == "성공" || json.msg == "Success")
                    {
                        Session.session.isGeust=false;
                        Session.session.Login(json);
                        SceneManager.LoadScene(Session.session.HexCode.IsNullOrEmpty() ? 2 : 3);
                    }
                    else
                    {
                        msg.text = json.msg;
                        isSignIn = true;
                    }

                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                    msg.color = new Color(1, 0, 0);
                    msg.text = "Login failed";
                    isSignIn = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "Sign up failed. (Server connection error)";
                isSignIn = true;
            }
        }
    }
    IEnumerator Test()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Env.Api("")))
        {
            yield return www.SendWebRequest();
            Debug.Log("서버 응답 내용: " + www.downloadHandler.text);
        }
    }
}
