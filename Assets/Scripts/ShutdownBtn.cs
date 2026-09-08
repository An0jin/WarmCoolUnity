using UnityEngine; // Unity 기본 엔진 네임스페이스 참조

/// <summary>버튼 또는 뒤로가기 입력으로 애플리케이션을 종료합니다.</summary>
public class ShutdownBtn : Btn // Btn 추상 클래스를 상속받는 앱 종료 버튼 클래스
{
    // 버튼 클릭 시 호출되는 메서드 재정의 (남는 씬 스택이 있더라도 강제 종료 처리)
    protected override void OnClick()
    {
        Application.Quit(); // 애플리케이션 완전히 종료
    }

    // 매 프레임 키 입력을 감지하는 Unity Update 생명주기 메서드
    void Update()
    {
        // 안드로이드 뒤로가기 버튼 또는 키보드 ESC 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick(); // 앱 종료 실행
        }
    }
}
