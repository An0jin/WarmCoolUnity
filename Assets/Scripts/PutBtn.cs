using System;
using Toneiverse;
using Toneiverse.DTO;
using UnityEngine;
using UnityEngine.UI;

public class PutBtn : Btn
{
    bool isUpdate;
    [SerializeField] InputField name, pw, check;
    [SerializeField] Text msg;
    void Awake()
    {
        isUpdate = true;
        base.Awake();
    }
    protected override void OnClick()
    {
        if (isUpdate)
        {
            isUpdate = false;
            msg.color = new Color(1, 1, 1);
            msg.text = "수정중";
            isUpdate = false;
            if (pw.text == "" || name.text == "")
            {
                msg.color = new Color(1, 0, 0);
                msg.text = "모든정보를 입력해주세요";
                isUpdate = true;
                return;//끝내기
            }
            if (!Validator.MatchPw(pw.text))
            {
                msg.color = new Color(1, 0, 0);
                msg.text = "패스워드는 영문과 숫자, 특수문자로 구성되어야 하며 8~16자리여야 합니다.";
                isUpdate = true;
                return;
            }
            if (pw.text != check.text)
            {
                msg.color = new Color(1, 0, 0);
                msg.text = "패스워드를 다시 확인해주세요.";
                isUpdate = true;
                return;
            }
            UserInfo user = new UserInfo()
            {
                name = name.text,
                pw = pw.text,
                token = Session.session.Token
            };
            StartCoroutine(APIManager.Put("user", JsonUtility.ToJson(user), (sussess) =>
            {
                try
                {
                    Json<string> json = JsonUtility.FromJson<Json<string>>(sussess);
                    Debug.Log("JSON 파싱 결과: " + JsonUtility.ToJson(json));
                    Session.session.UpdateInfo(name.text);
                    msg.text = json.result;
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                    msg.text = "수정 실패. (응답 처리 오류)";
                    isUpdate = true;
                }

            }, (err) =>
            {
                Debug.LogError("웹 요청 오류: " + err);
                msg.text = "수정 실패. (서버 연결 오류)";
                isUpdate = true;
            }));
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
public class UserInfo
{
    public string name, pw, token;
}
