using UnityEngine; // Unity 기본 엔진 네임스페이스 참조

/// <summary>환경 설정 에셋을 전역에서 제공하는 씬 유지형 싱글턴입니다.</summary>
public class Env : MonoBehaviour // 전역 설정 에셋에 접근을 제공하는 씬 유지형 싱글턴 클래스
{
    // 전역 정적 인스턴스 (Env.I 로 접근)
    public static Env I { get; private set; }

    [SerializeField] private EnvConfig config; // 유니티 에디터 인스펙터에서 연결하는 EnvConfig ScriptableObject

    // 환경 설정 에셋 객체를 읽기 전용으로 반환하는 프로퍼티
    public EnvConfig Config => config;

    // 초기화 생명주기 메서드
    private void Awake()
    {
        // 중복 생성된 Env 인스턴스가 존재한다면 제거하여 싱글턴 유지
        if (I != null && I != this)
        {
            Destroy(gameObject); // 중복 오브젝트 파괴
            return;
        }
        I = this; // 정적 인스턴스 저장

        // 씬이 변경되더라도 싱글턴 오브젝트가 유지되도록 등록
        DontDestroyOnLoad(gameObject);
    }
}
