using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Events; // UnityEvent 네임스페이스 참조
using Toneiverse.DTO; // 서버 데이터 전달 객체(DTO) 네임스페이스 참조

/// <summary>로그인 사용자와 퍼스널 컬러 정보를 씬 사이에서 공유합니다.</summary>
public class Session : MonoBehaviour // 사용자 상태를 세션 형태로 싱글턴 관리하는 클래스
{
    private static Session _instance; // 세션 인스턴스를 보관하는 정적 변수

    // 전역 접근을 위한 정적 프로퍼티
    public static Session session
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 기존 Session 객체 검색
                _instance = FindFirstObjectByType<Session>();

                if (_instance == null)
                {
                    // 씬에 없으면 새로 생성 후 컴포넌트 추가
                    GameObject singletonObject = new GameObject(typeof(Session).Name);
                    _instance = singletonObject.AddComponent<Session>();
                }

                DontDestroyOnLoad(_instance.gameObject); // 씬 변경 시 파괴 방지
            }
            return _instance;
        }
    }

    // 세션 프로퍼티 정의 (인증 토큰, 사용자 이름, 성별, 연도, 이메일, 진단 ID 등)
    public string Token { get; private set; } // JWT 인증 토큰
    public string Name { get; private set; } // 사용자 이름
    public string Sex { get; private set; } // 사용자 성별
    public string Year { get; private set; } // 출생 연도
    public string Email { get; private set; } // 이메일 주소
    public string ColorId { get; private set; } // 쿨톤/웜톤 퍼스널컬러 ID
    public string Cname { get; set; } // 립스틱/색상 명칭

    private string _hexCode; // HEX 색상 코드 비공개 필드

    // HEX 색상 코드 프로퍼티 (값 변경 시 이벤트 전송)
    public string HexCode
    {
        set
        {
            if (_hexCode != value)
            {
                _hexCode = value;
                onColorChangedEvent?.Invoke(); // 색상 변경 통지 UnityEvent 호출
            }
        }
        get => _hexCode;
    }

    [Header("세션 상태 변경 이벤트")]
    [SerializeField] public UnityEvent onColorChangedEvent = new UnityEvent();
    [SerializeField] public UnityEvent<InfoJson> onLoggedInEvent = new UnityEvent<InfoJson>();
    [SerializeField] public UnityEvent onLoggedOutEvent = new UnityEvent();
    [SerializeField] public UnityEvent<ColorJson> onColorPredictedEvent = new UnityEvent<ColorJson>();
    [SerializeField] public UnityEvent<string, string, string> onProfileUpdatedEvent = new UnityEvent<string, string, string>();

    // 스크립트 연결을 위한 간편 접근자
    public static UnityEvent OnColorChanged => session.onColorChangedEvent;
    public static UnityEvent<InfoJson> OnLoggedIn => session.onLoggedInEvent;
    public static UnityEvent OnLoggedOut => session.onLoggedOutEvent;
    public static UnityEvent<ColorJson> OnColorPredicted => session.onColorPredictedEvent;
    public static UnityEvent<string, string, string> OnProfileUpdated => session.onProfileUpdatedEvent;

    // 오브젝트 생성 및 초기화
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // 중복 객체 파괴
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
    }

    // 로그인 시 DTO 객체의 정보를 세션에 채움
    public void Login(InfoJson json)
    {
        Name = json.name;
        Email = json.email;
        ColorId = json.color_id;
        HexCode = json.hex_code;
        Token = json.token;
        Cname = json.cname;
        Sex = json.sex;
        Year = json.year;
        onLoggedInEvent?.Invoke(json); // 로그인 이벤트 통지
    }

    // 로그아웃 시 씬 이동 기록 및 세션 데이터 초기화
    public void LogOut()
    {
        NavigationManager.navigationManager.ClearHistory();
        Name = "";
        Email = "";
        ColorId = "";
        HexCode = "";
        Token = "";
        Cname = "";
        Sex = "";
        Year = "";
        onLoggedOutEvent?.Invoke(); // 로그아웃 이벤트 통지
    }

    // 가입 정보 설정
    public void SignIn(string name, string email)
    {
        Name = name;
        Email = email;
    }

    // 프로필 정보 업데이트
    public void UpdateInfo(string name, string sex, string year, string token)
    {
        Name = name;
        Sex = sex;
        Year = year;
        Token = token;
        onProfileUpdatedEvent?.Invoke(name, sex, year); // 프로필 갱신 이벤트 통지
    }

    // 성별 및 출생연도 설정
    public void SetProfile(string sex, string year)
    {
        Sex = sex;
        Year = year;
    }

    // 퍼스널컬러 진단 결과 적용
    public void Predict(ColorJson json)
    {
        HexCode = json.hex_code;
        ColorId = json.color_id;
        Cname = json.cname;
        onColorPredictedEvent?.Invoke(json); // 퍼스널컬러 진단 완료 이벤트 통지
    }
}
