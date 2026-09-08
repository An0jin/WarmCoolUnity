using UnityEngine;
using UnityEngine.UI;
using Toneiverse;
using Toneiverse.DTO;
using System;

public class ProfileSetupBtn : MSGBtn
{
    [SerializeField] private Toggle man;
    [SerializeField] private InputField year;
    private bool isUpdate = true;

    protected override void OnClick()
    {
        if (!isUpdate) return;

        isUpdate = false;
        Session.session.SetProfile(man.isOn ? "남자" : "여자", year.text);

        if (string.IsNullOrEmpty(year.text))
        {
            Error("출생 연도를 입력해주세요.");
            isUpdate = true;
            return;
        }

        int currentYear = DateTime.Now.Year;
        if (!int.TryParse(year.text, out int birth) || currentYear - birth < 1 || currentYear - birth > 120)
        {
            Error("태어난 연도가 이상합니다");
            isUpdate = true;
            return;
        }

        ProfileSetupJson payload = new ProfileSetupJson
        {
            token = Session.session.Token,
            sex = Session.session.Sex,
            year = Session.session.Year
        };

        StartCoroutine(APIManager.Put("user", JsonUtility.ToJson(payload), (jsonText) =>
        {
            try
            {
                Json<string> json = JsonUtility.FromJson<Json<string>>(jsonText);
                Debug.Log("JSON 파싱 결과: " + JsonUtility.ToJson(json));
                if (json.result == "수정 완료")
                {
                    NavigationManager.navigationManager.Front(string.IsNullOrEmpty(Session.session.HexCode) ? SceneIndex.Test : SceneIndex.Result);
                }
                else
                {
                    Error("수정 실패. (응답 처리 오류)");
                    isUpdate = true;
                }
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
