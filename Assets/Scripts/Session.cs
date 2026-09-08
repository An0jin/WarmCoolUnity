using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using Toneiverse.DTO; // 서버 데이터 전달 객체(DTO) 네임스페이스 참조
using System; // Action 델리게이트 등 C# 기본 네임스페이스 참조

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
                OnColorChanged?.Invoke(); // 색상 변경 통지 이벤트 호출
            }
        }
        get => _hexCode;
    }

    public static Action OnColorChanged; // 색상 변경 이벤트 델리게이트

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
    }
}
