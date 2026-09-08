using System.IO; // Path 클래스 등 파일 경로 작업을 위한 네임스페이스 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조

[CreateAssetMenu(fileName = "Env", menuName = "Env/Setting")] // 유니티 에셋 생성 메뉴 추가 (Create -> Env -> Setting)
/// <summary>API, Photon, 로컬 저장 파일에 필요한 실행 환경 값을 보관합니다.</summary>
public class EnvConfig : ScriptableObject // 설정 데이터를 파일로 보관하는 ScriptableObject 기반 클래스
{
    [Header("API Settings")] // 에디터 인스펙터 카테고리 헤더
    [SerializeField] private string api; // 백엔드 서버 기본 URL 주소 필드

    // 엔드포인트 경로를 받아 기본 URL과 안전하게 결합하는 메서드
    public string Api(string endpoint)
    {
        // 시작 부분의 '/' 여부를 체크하여 올바른 URL 조합 반환
        return api + (endpoint.StartsWith("/") ? endpoint.Substring(1) : endpoint);
    }

    [Header("Photon Settings")] // 포톤 네트워크 카테고리 헤더
    [SerializeField] private string photonChatid; // Photon Chat 애플리케이션 ID
    [SerializeField] private string photonAppid; // Photon Realtime 애플리케이션 ID

    [Header("user data file")] // 유저 데이터 파일 카테고리 헤더
    [SerializeField] private string fname; // 저장할 인증 토큰 파일 이름

    // Photon Chat ID 읽기 프로퍼티
    public string PhotonChatId => photonChatid;

    // Photon Realtime App ID 읽기 프로퍼티
    public string PhotonAppId => photonAppid;

    // 플랫폼별 영구 저장 디렉터리 경로와 파일 이름을 결합한 절대 경로
    public string FilePath => Path.Combine(Application.persistentDataPath, fname);
}
