using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // Unity UI(Text 컴포넌트) 네임스페이스 참조

/// <summary>성공과 오류 메시지를 일관된 색상으로 표시하는 버튼 기본 클래스입니다.</summary>
public abstract class MSGBtn : Btn // Btn 클래스를 상속받고 메시지 출력을 지원하는 추상 클래스
{
    [SerializeField] protected Text msg; // 메시지를 출력할 UI Text 컴포넌트 참조
    [SerializeField] protected Color color = new Color(250 / 255f, 156 / 255f, 120 / 255f); // 성공 상태 텍스트의 기본 지정 색상

    // 메시지 텍스트와 색상을 적용하는 비공개 헬퍼 함수
    private void SetMSG(string text, Color color)
    {
        msg.color = color; // Text UI의 색상 지정
        msg.text = text; // Text UI의 내용 텍스트 업데이트
    }

    // 성공 안내 텍스트를 출력하는 가상 메서드
    protected virtual void Success(string text)
    {
        SetMSG(text, color); // 설정된 성공 색상으로 안내 텍스트 표시
    }

    // 오류 안내 텍스트를 출력하는 가상 메서드
    protected virtual void Error(string text)
    {
        SetMSG(text, Color.red); // 빨간색으로 에러 내용 표시
    }
}
