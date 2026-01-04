using UnityEngine;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{
    private static ChatManager _instance;
    public static ChatManager chatManager
    {
        get
        {
            if (_instance == null)
            {
                // 최신 유니티 권장 메서드 사용
                _instance = FindFirstObjectByType<ChatManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ChatManager");
                    _instance = obj.AddComponent<ChatManager>();
                }
            }
            return _instance;
        }
    }
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

    public string PhotonChatId
    {
        set
        {
            if (PhotonNetwork.PhotonServerSettings != null)
            {
                PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat = value;
                Debug.Log($"[ChatManager] Chat ID Set: {value}"); // 로그 기록 습관
            }
        }
    }
    // 플래그 변수 없이도 더 정확하게 확인하는 법
    public bool IsAppIdConfigured =>
        PhotonNetwork.PhotonServerSettings != null &&
        !string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime) && !string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat);

    public string PhotonAppId
    {
        set
        {
            if (PhotonNetwork.PhotonServerSettings != null)
            {
                PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = value;
                Debug.Log($"[ChatManager] App ID Set: {value}");
            }
        }
    }
}