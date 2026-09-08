using Toneiverse; // 프로젝트 열거형(SceneIndex 등) 네임스페이스 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조

/// <summary>클릭하면 인스펙터에서 지정한 씬으로 이동합니다.</summary>
public class SceneBtn : Btn // Btn 추상 클래스를 상속받는 씬 이동 버튼 클래스
{
    [SerializeField] protected SceneIndex scene; // 인스펙터에서 이동할 목적지 씬을 지정하는 변수

    // 부모 클래스의 OnClick 추상 메서드 재정의
    protected override void OnClick()
    {
        NavigationManager.navigationManager.Front(scene); // NavigationManager를 사용하여 대상 씬으로 이동 (스택 기록)
    }
}
