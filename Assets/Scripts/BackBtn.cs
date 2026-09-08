using UnityEngine; // Unity 기본 엔진 네임스페이스 참조

/// <summary>버튼 클릭 또는 뒤로가기 입력으로 이전 화면을 엽니다.</summary>
public class BackBtn : Btn // Btn 추상 클래스를 상속받는 뒤로가기 버튼 클래스
{
    // 매 프레임마다 호출되는 Unity 생명주기 메서드
    void Update()
    {
        // 안드로이드 뒤로가기 버튼이나 ESC 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick(); // 버튼 클릭 시 실행되는 메서드 호출
        }
    }

    // 부모 클래스의 OnClick 추상 메서드 재정의
    protected override void OnClick()
    {
        NavigationManager.navigationManager.Back(); // NavigationManager 싱글턴을 통해 이전 씬으로 이동
    }
}
