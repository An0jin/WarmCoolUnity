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
using Unity.VisualScripting;

public class SignUp : FormBtn
{
    [SerializeField] InputField id;
    [SerializeField] Dropdown domain;
    [SerializeField] GetNum numBtn;
    [SerializeField] Toggle agree;
    [SerializeField] InputField num;
    bool isSignUp;

    // Start is called before the first frame update
    void Awake()
    {
        isSignUp = true;
        base.Awake();
    }
    string email => $"{id.text.Trim()}@{domain.options[domain.value].text.ToLower()}";
    protected override bool ValidateForm()
    {// 1. 부모 클래스 검증(공백, 정규식 등) 실패 시 즉시 중단
        if (!base.ValidateForm())
            return false;
        if (!agree.isOn)
        {
            Error("개인정보처리방침을 동의해주세요.");
            return false;
        }

        // 2. 인증번호 일치 여부 확인
        if (!numBtn.CheckNum(num.text))
        {
            Error("인증번호가 일치하지 않습니다.");
            return false;
        }

        // 3. 인증받은 이메일과 현재 입력된 이메일 일치 여부 확인
        if (!numBtn.CheckEmail(email)) // 여기서 email은 앞서 만든 FullEmail 프로퍼티 권장
        {
            Error("이메일이 수정되었습니다. 다시 인증해주세요.");
            return false;
        }

        return true; // 모든 관문을 통과해야만 true 반환
    }
    protected override bool IsNull()
    {
        return base.IsNull() || string.IsNullOrEmpty(id.text) || string.IsNullOrEmpty(num.text);
    }
    protected override void OnClick()
    {
        if (isSignUp)
        {
            if (!ValidateForm())
            {
                isSignUp = true;
                return;
            }
            Success("회원가입 중...");
            isSignUp = false;

            WWWForm form = new WWWForm();
            form.AddField("pw", pw.text);
            form.AddField("name", name.text);
            form.AddField("email", email);
            form.AddField("year", year.text);
            form.AddField("sex", sex);
            StartCoroutine(APIManager.Post("user", form, (susses) =>
            {
                try
                {
                    SignUpJson json = JsonUtility.FromJson<SignUpJson>(susses);
                    if (string.IsNullOrEmpty(json.result))//result가 비어있으면 성공이다
                    {
                        Token token = new Token();
                        token.token = json.token;
                        File.WriteAllText(Env.I.Config.FilePath, JsonUtility.ToJson(token));
                        Session.session.SignIn(name.text, email);
                        SceneManager.LoadScene((int)SceneIndex.Test);
                    }
                    else
                    {
                        Error(json.result);
                        isSignUp = true;
                    }
                }
                catch (Exception e)
                {
                    Error("JSON 파싱 오류: " + e.Message);
                    isSignUp = true;
                }

            }, (error) =>
            {
                Error("웹 요청 오류: " + error);
                isSignUp = true;

            }));
        }

    }

}
