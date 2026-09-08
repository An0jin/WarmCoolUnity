using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // Unity UI Text 컴포넌트 네임스페이스 참조

/// <summary>Text 컴포넌트를 찾아 화면별 문자열을 설정하는 기본 클래스입니다.</summary>
public abstract class TXT : MonoBehaviour // UI Text 컴포넌트 제어를 자동화하는 기본 추상 클래스
{
    protected Text text; // UI Text 컴포넌트 참조 변수

    // 초기화 시 호출되는 Unity 생명주기 메서드
    protected virtual void Awake()
    {
        text = GetComponent<Text>(); // 동일 게임 오브젝트의 Text 컴포넌트를 가져옴
        SetText(); // 화면에 맞는 텍스트 설정 메서드 실행
    }

    // 자식 클래스에서 구체적인 텍스트 설정 로직을 정의하는 추상 메서드
    public abstract void SetText();
}
