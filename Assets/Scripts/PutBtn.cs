using System;
using Toneiverse;
using Toneiverse.DTO;
using UnityEngine;
using UnityEngine.UI;

public class PutBtn : FormBtn
{
    bool isUpdate;
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
            Success("수정중...");
            if (!ValidateForm())
            {
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

    public class UserInfo
    {
        public string name, pw, token;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}