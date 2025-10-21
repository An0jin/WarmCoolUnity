using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SignUp : MonoBehaviour
{
    [SerializeField]Button signUP;
    [SerializeField]InputField id, name, pw,email;
    bool isSignUp;
    [SerializeField]Toggle man;
    [SerializeField]Text msg;
    // Start is called before the first frame update
    void Start()
    {
        isSignUp = true;
        signUP.onClick.AddListener(() =>
        {
            if (isSignUp)
                StartCoroutine(SignUP());
        });
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {    
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(0);
        }
    
    }
    IEnumerator SignUP()
    {
        string idPattern = "^[a-zA-Z0-9]{8,16}$";
        string pwPattern = "^[a-zA-Z0-9`~!@#$%^&*()_\\-+=\\[\\]{}|;:'\",<.>/?]{8,16}$";
        string emailPattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.(com|net|org|kr)$";
        string gender = man.isOn ? "Male" : "Female";
        msg.color = new Color(1, 1, 1);
        msg.text = "회원가입 중...";
        isSignUp = false;
        if (id.text == "" || pw.text == "" || email.text == "" || name.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "모든 정보를 입력해주세요.";
            isSignUp = true;
            yield break;//끝내기
        }
        if (!Regex.IsMatch(email.text, emailPattern))
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "이메일이 이상합니다";
            isSignUp = true;
            yield break;//끝내기
        }
        if (!Regex.IsMatch(id.text, idPattern))
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "ID는 영문과 숫자로 구성되어야 하며 8~16자리여야 합니다.";
            isSignUp = true;
            yield break;
        }
        if (!Regex.IsMatch(pw.text, pwPattern))
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "비밀번호는 영문과 숫자, 특수문자로 구성되어야 하며 8~16자리여야 합니다.";
            isSignUp = true;
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", id.text);
        form.AddField("pw", pw.text);
        form.AddField("name", name.text);
        form.AddField("email",email.text);
        form.AddField("gender", gender);
        using (UnityWebRequest www = UnityWebRequest.Post(Env.Api("user"), form))
        {
            yield return www.SendWebRequest();

            // 디버그 로그 추가
            Debug.Log("서버 응답 코드: " + www.responseCode);
            Debug.Log("서버 응답 내용: " + www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    Json<string> json = JsonUtility.FromJson<Json<string>>(www.downloadHandler.text);
                    if (json.result == "Sign up complete")
                    {

                        Session.session.SignIn(id.text, name.text, gender, email.text);
                        Session.session.isGeust = false;
                        SceneManager.LoadScene(2);
                    } else
                    {
                    msg.color = new Color(1, 0, 0);
                    msg.text = "Sign up failed. (응답 처리 오류)";
                    isSignUp = true;

                    
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                    msg.color = new Color(1, 0, 0);
                    msg.text = "Sign up failed. (응답 처리 오류)";
                    isSignUp = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "Sign up failed. (서버 연결 오류)";
                isSignUp = true;
            }
        }
    }
}
