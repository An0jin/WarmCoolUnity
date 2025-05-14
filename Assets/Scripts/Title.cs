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
        print("버튼을 눌렀다");
        isSignIn = false;
        msg.color = new Color(1, 1, 1);
        msg.text = "로그인 하는중";
        if (id.text == "" || pw.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "아이디와 비밀번호를 입력해주세요";
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
                    if (json.msg == "성공")
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
                    msg.text = "로그인 실패";
                    isSignIn = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "회원가입에 실패했습니다. (서버 연결 오류)";
                isSignIn = true;
            }
        }
    }
}
