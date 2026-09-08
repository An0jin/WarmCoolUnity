using System.Collections; // IEnumerator 코루틴 참조
using System.IO; // File 로컬 저장 파일 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // 네트워크 관련 참조
using UnityEngine.SceneManagement; // 씬 관리 참조
using UnityEngine.UI; // Text UI 참조
using Toneiverse.DTO; // DTO 객체 참조
using Toneiverse; // SceneIndex 참조
using System; // Exception 예외 처리 참조

/// <summary>저장된 토큰으로 자동 로그인하고 적절한 시작 씬으로 이동합니다.</summary>
public class Loading : MonoBehaviour // 타이틀 로딩 씬에서 버전 체크 및 자동 로그인을 전담하는 클래스
{
    [SerializeField] Text msg; // 로딩 상황 메시지 UI
    [SerializeField] GameObject susses; // 로그인/회원가입 수동 이동 버튼 그룹 오브젝트

    // 첫 프레임 초기화
    void Start()
    {
        CheckVersion(); // 버전 상태 체크 시작
        SetLoading(true); // 로딩 전용 UI 표출
    }

    // 로딩 토글 상태 조절
    void SetLoading(bool show)
    {
        susses.SetActive(!show);
        msg.gameObject.SetActive(show);
    }

    // 뒤로가기 키 처리
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // 로컬 저장 인증 토큰 기반 자동 로그인 수행
    void CheckAutoLogin()
    {
        msg.text = "자동 로그인을 체크하는중...";
        if (File.Exists(Env.I.Config.FilePath))
        {
            string data = File.ReadAllText(Env.I.Config.FilePath);
            if (string.IsNullOrEmpty(data))
            {
                SetLoading(false);
                return;
            }
            Token token = JsonUtility.FromJson<Token>(data);
            print("token : " + token.token);
            if (string.IsNullOrEmpty(token.token))
            {
                SetLoading(false);
                return;
            }
            else
            {
                // 서버 토큰 유효성 검증
                StartCoroutine(APIManager.Get($"/user/{token.token}", (jsonText) =>
                {
                    InfoJson json = JsonUtility.FromJson<InfoJson>(jsonText);
                    print("이메일 : " + json.email);
                    if (string.IsNullOrEmpty(json.email))
                    {
                        SetLoading(false);
                        File.Delete(Env.I.Config.FilePath); // 유효하지 않으면 삭제
                    }
                    else
                    {
                        Session.session.Login(json); // 세션 데이터 적재
                        
                        // 프로필 미작성 / 진단 미실행 / 진단 완료 상태별 목적지 씬 분기
                        NavigationManager.navigationManager.Front(
                            string.IsNullOrEmpty(Session.session.Sex) ? SceneIndex.ProfileSetup : 
                            string.IsNullOrEmpty(Session.session.HexCode) ? SceneIndex.Test : 
                            SceneIndex.Result
                        );
                    }
                }));
            }
        }
        else
        {
            SetLoading(false); // 로컬 데이터 없으면 수동 로그인 표시
        }
    }

    // 앱 버전 호환성 체크
    void CheckVersion()
    {
        msg.text = "버전 체크중...";

        StartCoroutine(APIManager.Get($"/version/{Application.version}", (jsonText) =>
        {
            if (string.IsNullOrEmpty(jsonText))
            {
                Debug.LogError("Server Response is Empty");
                msg.text = "서버 점검중이거나 서버에 문제가 생겼습니다";
                return;
            }

            Json<bool> json = JsonUtility.FromJson<Json<bool>>(jsonText);

            if (json == null)
            {
                throw new Exception("JSON Parsing returned null");
            }

            if (json.result)
            {
                CheckAutoLogin(); // 버전에 문제없으면 자동 로그인 진행
            }
            else
            {
                // 구버전인 경우 마켓 이동 후 종료
                string url = "";
#if UNITY_IOS
                url = "나중에 만들예정";
#else
                url = "https://play.google.com/store/apps/details?id=com.an0jin.Toneiverse";
#endif
                Application.OpenURL(url);
                msg.text = "업데이트 필요";
                Application.Quit();
            }
        }));
    }
}
