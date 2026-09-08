using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using Photon.Pun; // Photon PUN 네임스페이스 참조

/// <summary>Photon 앱 설정을 관리하며 씬 전환 후에도 유지되는 싱글턴입니다.</summary>
public class ChatManager : MonoBehaviour // 포톤 채팅 네트워크 설정을 동적으로 주입하는 싱글턴
{
    private static ChatManager _instance; // 정적 인스턴스 필드

    // 전역 접근용 싱글턴 프로퍼티
    public static ChatManager chatManager
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<ChatManager>(); // 기존 객체 탐색
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ChatManager"); // 동적 오브젝트 생성
                    _instance = obj.AddComponent<ChatManager>();
                }
            }
            return _instance;
        }
    }

    // 오브젝트 초기화
    private void Awake()
    {
        // 1. 중복 인스턴스 파괴 및 씬 전환 시 유지 설정
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Photon Chat 서비스용 App ID 주입 프로퍼티
    public string PhotonChatId
    {
        set
        {
            if (PhotonNetwork.PhotonServerSettings != null)
            {
                PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat = value;
                Debug.Log($"[ChatManager] Chat ID Set: {value}"); // 로그 출력
            }
        }
    }

    // App ID 설정 여부를 검증하는 프로퍼티
    public bool IsAppIdConfigured =>
        PhotonNetwork.PhotonServerSettings != null &&
        !string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime) &&
        !string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat);

    // Photon Realtime 서비스용 App ID 주입 프로퍼티
    public string PhotonAppId
    {
        set
        {
            if (PhotonNetwork.PhotonServerSettings != null)
            {
                PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = value;
                Debug.Log($"[ChatManager] App ID Set: {value}"); // 로그 출력
            }
        }
    }
}
