using UnityEngine;
using UnityEngine.UI;
using Toneiverse;
using Toneiverse.DTO;
using System;

public class ProfileSetupBtn : MSGBtn
{// [UI 연결] 성별 선택과 출생 연도 입력 필드
    [SerializeField] private Toggle man;
    [SerializeField] private InputField year;
    bool isUpdate = true;
    public class TEMP
    {
        public string token;
        public string sex;
        public string year;
    }
    protected override void OnClick()
    {
        if (isUpdate)
        {

            isUpdate = false;
            if (man.isOn)
                Session.session.AAA("남자", year.text);
            else
                Session.session.AAA("여자", year.text);
            if (string.IsNullOrEmpty(year.text))
            {
                Error("출생 연도를 입력해주세요.");
                return;
            }
            int todayTear = DateTime.Now.Year;
            int birth = int.Parse(year.text);
            if (1 > todayTear - birth || todayTear - birth > 120)
            {
                Error("태어난 연도가 이상합니다");
                return;
            }
            TEMP temp = new TEMP()
            {
                token = Session.session.Token,
                sex = Session.session.Sex,
                year = Session.session.Year
            };
            StartCoroutine(APIManager.Put("user", JsonUtility.ToJson(temp), (sussess) =>
            {
                try
                {
                    Json<string> json = JsonUtility.FromJson<Json<string>>(sussess);
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
}
