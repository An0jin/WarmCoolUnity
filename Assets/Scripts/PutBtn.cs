using System;
using System.IO;
using Toneiverse;
using Toneiverse.DTO;
using UnityEngine;
using UnityEngine.UI;

public class PutBtn : FormBtn
{
    bool isUpdate;
    [SerializeField] Toggle woman;
    void Awake()
    {
        if (Session.session.Sex == "남자")
            man.isOn = true;
        else
            woman.isOn = true;
        name.text = Session.session.Name;
        year.text = Session.session.Year;
        isUpdate = true;
        base.Awake();
    }

    protected override void OnClick()
    {
        print($"눌렀다");

        if (isUpdate)
        {
            print($"체크완료");

            isUpdate = false;
            Success("수정중...");
            if (!ValidateForm())
            {
                isUpdate = true;
                return;
            }
            print($"pw.text : {pw.text}");
            UserInfo user = new UserInfo()
            {
                name = name.text,
                pw = pw.text,
                token = Session.session.Token,
                sex = sex,
                year = year.text
            };
            StartCoroutine(APIManager.Put("user", JsonUtility.ToJson(user), (sussess) =>
            {
                try
                {
                    PutJson json = JsonUtility.FromJson<PutJson>(sussess);
                    Debug.Log("JSON 파싱 결과: " + JsonUtility.ToJson(json));
                    Session.session.UpdateInfo(name.text, sex, year.text, json.token);
                    Token token = new Token();
                    token.token = json.token;
                    File.WriteAllText(Env.I.Config.FilePath, JsonUtility.ToJson(token));
                    Success(json.result);
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                    Error("수정 실패. (응답 처리 오류)");
                    isUpdate = true;
                }

            }, (err) =>
            {
                Debug.LogError("웹 요청 오류: " + err);
                Error("수정 실패. (서버 연결 오류)");
                isUpdate = true;
            }));
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}