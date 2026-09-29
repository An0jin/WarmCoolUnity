using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Events; // UnityEvent 네임스페이스 참조
using UnityEngine.UI; // Unity UI(Button, Text 등) 네임스페이스 참조

[RequireComponent(typeof(Button))] // 이 스크립트가 부착된 게임 오브젝트에 Button 컴포넌트가 반드시 존재하도록 강제함
/// <summary>Unity UI Button의 클릭 이벤트 연결을 표준화하고 인스펙터 UnityEvent를 제공하는 기본 클래스입니다.</summary>
public abstract class Btn : MonoBehaviour // 모든 버튼 클래스의 부모가 되는 추상 클래스 정의
{
    protected Button btn; // 자식 클래스에서 접근 가능하도록 버튼 컴포넌트 참조 변수 선언

    [Header("버튼 이벤트 설정")]
    [SerializeField] public UnityEvent onClickEvent = new UnityEvent(); // 인스펙터에서 바인딩 가능한 유니티 이벤트

    // 객체가 생성되고 초기화될 때 호출되는 Unity 생명주기 메서드
    protected virtual void Awake()
    {
        
        btn = GetComponent<Button>(); // 현재 오브젝트에서 Button 컴포넌트를 가져와 변수에 할당
        btn.onClick.AddListener(HandleClick); // 버튼 클릭 이벤트 발생 시 HandleClick 핸들러 실행
    }

    // 버튼 클릭 시 인스펙터 이벤트 발송 및 자식 클래스 로직 실행
    private void HandleClick()
    {
        OnClick(); // 자식 클래스 로직 수행
        onClickEvent?.Invoke(); // Unity 인스펙터 이벤트 발행
    }

    // 버튼이 클릭되었을 때 실행될 로직을 자식 클래스에서 구현하도록 강제하는 추상 메서드
    protected abstract void OnClick();
}
