using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField]InputField id, pw;
    [SerializeField]Button signIn,geust,signUp,getPw;
    bool isSignIn;
    [SerializeField]Text msg, loading;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckVersion());
        isSignIn = true;
        signIn.onClick.AddListener(() =>
        {
            if (isSignIn)
                StartCoroutine(LogIn());
        });
        geust.onClick.AddListener(() =>
        {
            Session.session.isGeust = true;
            SceneManager.LoadScene(2);
        });
        SetLoading(true);
    }
    void SetLoading(bool show){
        loading.gameObject.SetActive(show);
        id.gameObject.SetActive(!show);
        pw.gameObject.SetActive(!show);
        signIn.gameObject.SetActive(!show);
        signUp.gameObject.SetActive(!show);
        geust.gameObject.SetActive(!show);
        getPw.gameObject.SetActive(!show);
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
        msg.text = "로그인 중...";
        if (id.text == "" || pw.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "ID와 비밀번호를 입력해주세요.";
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
                        SceneManager.LoadScene((int)(string.IsNullOrEmpty(Session.session.HexCode) ? Scene.Test : Scene.Result));
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
                    msg.text = "로그인 실패. (응답 처리 오류)";
                    isSignIn = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "로그인 실패. (서버 연결 오류)";
                isSignIn = true;
            }
        }
    }
    IEnumerator CheckVersion()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Env.Api($"/version/{Application.version}")))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Json<bool> json = JsonUtility.FromJson<Json<bool>>(www.downloadHandler.text);
                if (json.result)
                {
                    SetLoading(false);
                }
                else
                {
                    string url = "";
                    #if UNITY_IOS
                        url = "나중에 만들예정";
                    #else
                        url = "https://play.google.com/store/apps/details?id=com.an0jin.Toneiverse";
#endif
                    Application.OpenURL(url);
                    loading.text = "업데이트 필요";
                    Application.Quit();
                }
            }
            else
            {
                msg.text="서버 정검중이거나 서버에 문제가 생겼습니다";
            }
        }
    }
}
