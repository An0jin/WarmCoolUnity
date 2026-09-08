using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // Toggle, InputField UI 컴포넌트 참조
using Toneiverse; // SceneIndex 열거형 참조
using Toneiverse.DTO; // DTO 구조체 참조
using System; // DateTime, Exception 참조

/// <summary>최초 사용자의 성별과 출생 연도를 검증해 서버에 저장합니다.</summary>
public class ProfileSetupBtn : MSGBtn // 프로필 초기 설정 전용 버튼 스크립트
{
    [SerializeField] private Toggle man; // 남성 토글 UI
    [SerializeField] private InputField year; // 연도 입력 UI
    private bool isUpdate = true; // 요청 진행 여부 플래그

    // 클릭 시 검증 및 통신 로직 실행
    protected override void OnClick()
    {
        if (!isUpdate) return; // 중복 클릭 시 차단

        isUpdate = false;
        Session.session.SetProfile(man.isOn ? "남자" : "여자", year.text); // 선택값 세션 동기화

        // 연도 미입력 시 예외 처리
        if (string.IsNullOrEmpty(year.text))
        {
            Error("출생 연도를 입력해주세요.");
            isUpdate = true;
            return;
        }

        // 나이 범위(1~120세) 유효성 계산
        int currentYear = DateTime.Now.Year;
        if (!int.TryParse(year.text, out int birth) || currentYear - birth < 1 || currentYear - birth > 120)
        {
            Error("태어난 연도가 이상합니다");
            isUpdate = true;
            return;
        }

        // 프로필 DTO 데이터 생성
        ProfileSetupJson payload = new ProfileSetupJson
        {
            token = Session.session.Token,
            sex = Session.session.Sex,
            year = Session.session.Year
        };

        // "user" API PUT 전송
        StartCoroutine(APIManager.Put("user", JsonUtility.ToJson(payload), (jsonText) =>
        {
            try
            {
                Json<string> json = JsonUtility.FromJson<Json<string>>(jsonText);
                Debug.Log("JSON 파싱 결과: " + JsonUtility.ToJson(json));
                if (json.result == "수정 완료")
                {
                    // 상태에 맞춰 Test(측정 씬) 또는 Result(결과 씬)으로 진입
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
