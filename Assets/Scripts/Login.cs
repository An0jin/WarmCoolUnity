using UnityEngine;
using Toneiverse.DTO;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using Toneiverse;
using System;
public class Login : Btn
{
    [SerializeField] InputField email, pw;
    [SerializeField] Text msg;

    protected override void OnClick()
    {
        msg.color = new Color(1, 1, 1);
        msg.text = "로그인 중...";
        if (email.text == "" || pw.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "이메일과 비밀번호를 입력해주세요.";
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("pw", pw.text);
        StartCoroutine(APIManager.Post("login", form, (jsonText) =>
        {
            try
            {
                InfoJson json = JsonUtility.FromJson<InfoJson>(jsonText);
                if (json.msg == "성공")
                {
                    Token token = new Token();
                    token.token = json.token;
                    File.WriteAllText(Env.I.Config.FilePath, JsonUtility.ToJson(token));
                    Session.session.Login(json);
                    NavigationManager.navigationManager.Front(string.IsNullOrEmpty(Session.session.Sex) ? SceneIndex.ProfileSetup : string.IsNullOrEmpty(Session.session.HexCode) ? SceneIndex.Test : SceneIndex.Result
);
                }
                else
                {
                    msg.text = json.msg;
                }

            }
            catch (Exception e)
            {
                Debug.LogError("JSON 파싱 오류: " + e.Message);
                msg.color = new Color(1, 0, 0);
                msg.text = "로그인 실패. (응답 처리 오류)";
            }
        }));
    }

}
