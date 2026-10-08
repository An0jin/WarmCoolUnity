using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // IMultipartFormSection 등 네트워킹 참조
using System.Collections.Generic; // List 제네릭 컬렉션 참조
using UnityEngine.UI; // InputField, Button, Text UI 참조
using Toneiverse.DTO; // DTO 객체 참조
using System; // OperationCanceledException 참조

/// <summary>사용자 질문을 전송해 AI 메이크업 답변을 표시하고 추천 결과를 적용합니다 (Unity 6 Awaitable).</summary>
public class LLM : MSGBtn // AI 메이크업 추천 프롬프트 전송 버튼
{
    [SerializeField] InputField prompt; // 프롬프트 질문 입력 필드
    [SerializeField] Button cls; // 닫기 버튼 참조
    [SerializeField] ResultText cnameText; // 결과 텍스트 갱신용 참조

    private string originalPlaceholder; // 기본 플레이스홀더 문구 저장 변수
    private Text placeholderComp; // 플레이스홀더 UI Text 컴포넌트

    // 컴포넌트 초기화
    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake(버튼 바인딩) 호출
        placeholderComp = prompt.placeholder.GetComponent<Text>();
        originalPlaceholder = placeholderComp.text;
    }

    // AI 질의 버튼 클릭 핸들러
    protected override async void OnClick()
    {
        if (string.IsNullOrEmpty(prompt.text)) return; // 공백 질문 예외 처리

        SetUIState(false); // UI 조작 잠금

        List<IMultipartFormSection> form = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("token", Session.session.Token),
            new MultipartFormDataSection("msg", prompt.text),
            new MultipartFormDataSection("sex", Session.session.Sex),
            new MultipartFormDataSection("year", Session.session.Year)
        };

        try
        {
            // API "llm" 호출 (Awaitable)
            LLMResponse colorJson = await APIManager.PostJsonAsync<LLMResponse>("llm", form);
            Session.session.HexCode = colorJson.hex_code; // 추천된 색상 헥스코드 반영
            Session.session.Cname = colorJson.cname; // 추천된 제품명 반영
            cnameText.SetText(); // UI 텍스트 업데이트
            Success(colorJson.result); // AI 텍스트 답변 표시
            prompt.text = ""; // 입력창 비우기
        }
        catch (OperationCanceledException) { }
        catch (Exception error)
        {
            Debug.LogError($"[LLM] 오류: {error.Message}");
        }
        finally
        {
            SetUIState(true); // 완료/취소/에러 시 UI 조작 잠금 해제
        }
    }

    // UI 대기/완료 상태 한 번에 조절
    private void SetUIState(bool isReady)
    {
        placeholderComp.text = isReady ? originalPlaceholder : "AI가 생각하고 있습니다.";
        prompt.interactable = isReady;
        btn.interactable = isReady;
        cls.interactable = isReady;
    }
}
