using System; // Exception 참조
using System.IO; // File 시스템 입출력 참조
using Toneiverse; // 프로젝트 네임스페이스 참조
using Toneiverse.DTO; // DTO 객체 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // UI Toggle, InputField 참조

/// <summary>현재 회원 정보를 표시하고 검증된 수정 내용을 서버에 저장합니다.</summary>
public class PutBtn : FormBtn // 회원정보 수정을 담당하는 스크립트
{
    bool isUpdate; // 수정 요청 진행 플래그
    [SerializeField] Toggle woman; // 여성 토글 UI

    // 폼 초기 데이터 세팅
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

    // 수정 버튼 클릭 시 실행되는 핸들러
    protected override void OnClick()
    {
        print($"눌렀다");

        if (isUpdate)
        {
            print($"체크완료");

            isUpdate = false;
            Success("수정중...");
            
            // 입력 데이터 유효성 다각도 검사
            if (!ValidateForm())
            {
                isUpdate = true;
                return;
            }
            print($"pw.text : {pw.text}");

            // 변경 유저 정보 DTO 구성
            UserInfo user = new UserInfo()
            {
                name = name.text,
                pw = pw.text,
                token = Session.session.Token,
                sex = sex,
                year = year.text
            };

            // 백엔드로 정보 수정 PUT 요청
            StartCoroutine(APIManager.Put("user", JsonUtility.ToJson(user), (jsonText) =>
            {
                try
                {
                    PutJson json = JsonUtility.FromJson<PutJson>(jsonText);
                    Debug.Log("JSON 파싱 결과: " + JsonUtility.ToJson(json));
                    
                    // 메모리 세션 및 로컬 인증 파일 갱신
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
}
