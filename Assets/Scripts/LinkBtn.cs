using UnityEngine; // Unity 기본 엔진 네임스페이스 참조

/// <summary>설정된 외부 웹 주소를 기본 브라우저로 엽니다.</summary>
public class LinkBtn : Btn // Btn 추상 클래스를 상속받는 외부 링크 연결 버튼 클래스
{
    // 부모 클래스의 OnClick 추상 메서드 재정의
    protected override void OnClick()
    {
        // URL 문자열이 비어있거나 null이 아닌지 검증
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url); // 기기의 기본 웹 브라우저로 지정된 URL 열기
    }

    // 인스펙터에서 설정 가능하고 외부에서는 쓰기만 허용하는 URL 프로퍼티
    [field: SerializeField] public string url { set; private get; }
}
