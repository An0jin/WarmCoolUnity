using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using System;

public class SignUp : MonoBehaviour
{
    [SerializeField]Button signUP,numBtn;
    [SerializeField] InputField name, pw, email, check, num;
    bool isSignUp;
    [SerializeField] Text msg;
    string snum;
    
    // Start is called before the first frame update
    void Start()
    {
        isSignUp = true;
        signUP.onClick.AddListener(() =>
        {
            if (isSignUp)
                StartCoroutine(SignUP());
        });
        numBtn.onClick.AddListener(() =>
        {
            StartCoroutine(GetNum());
        });
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
    IEnumerator GetNum()
    {
        msg.text = "";
        WWWForm form = new WWWForm();
        if (email.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "이메일을 입력해주세요.";
            yield break;
        }
        msg.color = new Color(1, 1, 1);
        msg.text = "인증번호를 생성하는 중...";
        form.AddField("email", email.text);
        snum = UnityEngine.Random.Range(0, 9999).ToString("D4");
        form.AddField("num", snum);
        using (UnityWebRequest www = UnityWebRequest.Post(Env.Api("getNum"), form))
        {
            yield return www.SendWebRequest();
            Json<string> json = JsonUtility.FromJson<Json<string>>(www.downloadHandler.text);
            if (www.result == UnityWebRequest.Result.Success)
            {
                msg.color = new Color(1, 1, 1);
                msg.text = json.result;
            }
            else
            {
                msg.color = new Color(1, 0, 0);
                msg.text = "인증번호 생성 실패.";
            }
        }   
    }
    IEnumerator SignUP()
    {
        string pwPattern = "^[a-zA-Z0-9`~!@#$%^&*()_\\-+=\\[\\]{}|;:'\",<.>/?]{8,16}$";
        string emailPattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.(com|net|org|kr)$";
        msg.color = new Color(1, 1, 1);
        msg.text = "회원가입 중...";
        isSignUp = false;
        if (pw.text == "" || email.text == "" || name.text == "")
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
        if (!Regex.IsMatch(pw.text, pwPattern))
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "비밀번호는 영문과 숫자, 특수문자로 구성되어야 하며 8~16자리여야 합니다.";
            isSignUp = true;
            yield break;
        }
        if (pw.text != check.text)
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "패스워드를 다시 확인해주세요.";
            isSignUp = true;
            yield break;
        }
        if(num.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "인증번호를 입력해주세요.";
            isSignUp = true;
            yield break;
        }
        if(num.text != snum)
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "인증번호가 일치하지 않습니다.";
            isSignUp = true;
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("pw", pw.text);
        form.AddField("name", name.text);
        form.AddField("email",email.text);
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
                    SignUpJson json = JsonUtility.FromJson<SignUpJson>(www.downloadHandler.text);
                    if (json.result == "Sign up complete")
                    {
                        Token token=new Token();
                        token.token=json.token;
                        File.WriteAllText(Env.filePath,JsonUtility.ToJson(token));
                        print(Env.filePath);
                        Session.session.SignIn(name.text, email.text);
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
