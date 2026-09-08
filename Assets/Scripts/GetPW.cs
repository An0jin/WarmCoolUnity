using System.Collections; // IEnumerator 코루틴 인터페이스 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // UnityEngine.Networking 네임스페이스 참조
using UnityEngine.UI; // InputField, Text UI 네임스페이스 참조
using Toneiverse.DTO; // DTO 구조체 참조

/// <summary>입력한 이메일로 비밀번호 찾기 요청을 전송합니다.</summary>
public class GetPW : Btn // Btn을 상속받는 비밀번호 찾기 기능 버튼
{
    [SerializeField] InputField email; // 이메일 입력 UI 필드 참조
    [SerializeField] Text msg; // 처리 결과 안내용 Text 컴포넌트

    // 버튼 클릭 이벤트 구현
    protected override void OnClick()
    {
        WWWForm form = new WWWForm(); // POST 통신용 폼 생성
        form.AddField("email", email.text); // 필드에 입력된 이메일 폼 데이터로 첨부
        msg.text = "아이디와 비밀번호 찾는중..."; // 로딩 안내 텍스트 표시

        // API 통신 전송
        StartCoroutine(APIManager.Post("email", form, (data) =>
        {
            Json<string> result = JsonUtility.FromJson<Json<string>>(data); // 서버 JSON 응답 파싱
            msg.text = result.result; // 서버 결과 문자열 표시
        }, (error) =>
        {
            msg.text = "로그인 실패. (서버 연결 오류)"; // 에러 발생 안내
        }));
    }
}
