using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using Toneiverse;
using Toneiverse.DTO;

public class SignUp : Btn
{
    [SerializeField] GetNum numBtn;
    [SerializeField] InputField name, pw, email, check, num;
    bool isSignUp;
    [SerializeField] Text msg;

    // Start is called before the first frame update
    void Awake()
    {
        isSignUp = true;
        base.Awake();
    }
    void Err(string errText)
    {
        msg.color = new Color(1, 0, 0);
        msg.text = errText;
        isSignUp = true;
    }
    private bool ValidateForm()
    {
        isSignUp = false;
        if (pw.text == "" || email.text == "" || name.text == "")
        {
            Err("모든 정보를 입력해주세요.");
            return false;
        }
        if (!Validator.MatchEmail(email.text))
        {
            Err("이메일이 이상합니다");
            return false;
        }
        if (!Validator.MatchPw(pw.text))
        {
            Err("비밀번호는 영문과 숫자, 특수문자로 구성되어야 하며 8~16자리여야 합니다.");
            return false;
        }
        if (pw.text != check.text)
        {
            Err("패스워드를 다시 확인해주세요.");
            return false;
        }
        if (num.text == "")
        {
            Err("인증번호를 입력해주세요.");
            return false;
        }
        if (!numBtn.CheckNum(num.text))
        {
            Err("인증번호가 일치하지 않습니다.");
            return false;
        }
        if (!numBtn.CheckEmail(email.text))
        {
            Err("이메일이 수정되었습니다. 다시 체크해주세요");
            return false;
        }
        return true;
    }
    protected override void OnClick()
    {
        if (!ValidateForm()) return;
        msg.color = new Color(1, 1, 1);
        msg.text = "회원가입 중...";
        isSignUp = false;

        WWWForm form = new WWWForm();
        form.AddField("pw", pw.text);
        form.AddField("name", name.text);
        form.AddField("email", email.text);
        StartCoroutine(APIManager.Post("user", form, (susses) =>
        {
            try
            {
                SignUpJson json = JsonUtility.FromJson<SignUpJson>(susses);
                if (string.IsNullOrEmpty(json.result))//result가 비어있으면 성공이다
                {
                    Token token = new Token();
                    token.token = json.token;
                    File.WriteAllText(Env.filePath, JsonUtility.ToJson(token));
                    Session.session.SignIn(name.text, email.text);
                    SceneManager.LoadScene(2);
                }
                else
                {
                    msg.color = new Color(1, 0, 0);
                    msg.text = json.result;
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

        }, (error) =>
        {
            Debug.LogError("웹 요청 오류: " + error);
            msg.color = new Color(1, 0, 0);
            msg.text = "Sign up failed. (서버 연결 오류)";
            isSignUp = true;

        }));

    }

}
