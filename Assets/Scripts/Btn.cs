using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // Unity UI(Button, Text 등) 네임스페이스 참조

[RequireComponent(typeof(Button))] // 이 스크립트가 부착된 게임 오브젝트에 Button 컴포넌트가 반드시 존재하도록 강제함
/// <summary>Unity UI Button의 클릭 이벤트 연결을 표준화하는 기본 클래스입니다.</summary>
public abstract class Btn : MonoBehaviour // 모든 버튼 클래스의 부모가 되는 추상 클래스 정의
{
    protected Button btn; // 자식 클래스에서 접근 가능하도록 버튼 컴포넌트 참조 변수 선언

    // 객체가 생성되고 초기화될 때 호출되는 Unity 생명주기 메서드
    protected virtual void Awake()
    {
        btn = GetComponent<Button>(); // 현재 오브젝트에서 Button 컴포넌트를 가져와 변수에 할당
        btn.onClick.AddListener(OnClick); // 버튼 클릭 이벤트가 발생했을 때 OnClick 메서드가 실행되도록 리스너 등록
    }

    // 버튼이 클릭되었을 때 실행될 로직을 자식 클래스에서 구현하도록 강제하는 추상 메서드
    protected abstract void OnClick();
}
