using System; // System 네임스페이스 참조
using System.Collections.Generic; // List 참조
using System.Linq; // LINQ 참조
using ExitGames.Client.Photon; // Photon SDK 참조
using Photon.Chat; // Photon Chat API 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // 네트워크 참조
using UnityEngine.SceneManagement; // 씬 전환 참조
using UnityEngine.UI; // InputField, Text UI 참조
using Toneiverse.DTO; // DTO 참조
using Photon.Pun; // Photon PUN 참조

/// <summary>이전 채팅을 불러오고 Photon 채널에서 실시간 메시지를 송수신합니다 (Unity 6 Awaitable).</summary>
public class Chat : Btn, IChatClientListener // 실시간 오픈 포톤 채팅 제어 스크립트
{
    private ChatClient chatClient; // Photon Chat 서비스 클라이언트 인스턴스
    [SerializeField] GameObject msgView; // 채팅 항목 부모 뷰
    [SerializeField] InputField input; // 입력 필드
    bool isConn; // 연결 플래그

    // 초기화 생명주기
    protected override void Awake()
    {
        base.Awake();
        isConn = false;

        // 포톤 서버 설정 자동 연결
        if (!ChatManager.chatManager.IsAppIdConfigured)
        {
            ChatManager.chatManager.PhotonChatId = Env.I.Config.PhotonChatId;
            ChatManager.chatManager.PhotonAppId = Env.I.Config.PhotonAppId;
        }
        chatClient = new ChatClient(this); // IChatClientListener 등록
        _ = GetChatAsync(); // 과거 DB 채팅 비동기 수신
        Application.runInBackground = true;
    }

    // 전송 버튼 클릭 핸들러
    protected override async void OnClick()
    {
        if (isConn && input.text.Replace(" ", "") != "")
        {
            string messageToSend = input.text;
            chatClient.PublishMessage(Session.session.ColorId, messageToSend); // 포톤 실시간 채팅 전송
            List<IMultipartFormSection> form = new List<IMultipartFormSection>
            {
                new MultipartFormDataSection("token", Session.session.Token),
                new MultipartFormDataSection("msg", messageToSend),
                new MultipartFormDataSection("color_id", Session.session.ColorId)
            };
            input.text = "";

            try
            {
                // DB 백엔드 채팅 내역 저장 요청 (Awaitable)
                await APIManager.PostAsync("chat", form);
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                Debug.LogError($"[Chat] 채팅 저장 오류: {e.Message}");
            }
        }
    }

    // DB에 저장된 기존 채팅 내역 가져오기 (Awaitable)
    private async Awaitable GetChatAsync()
    {
        try
        {
            var list = await APIManager.GetJsonAsync<JsonList<Message>>($"chat/{Session.session.ColorId}");
            if (list?.result != null)
            {
                foreach (Message item in list.result)
                {
                    AddMSG(item.name, item.msg);
                }
            }

            // 과거 채팅을 로드한 뒤 포톤 서버 접속 실행
            chatClient.Connect(Env.I.Config.PhotonChatId, "1.0", new AuthenticationValues(Session.session.Name));
            isConn = true;
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            Debug.LogError($"[Chat] 이전 채팅 로드 오류: {e.Message}");
        }
    }

    // 매 프레임 호출
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
            chatClient.Disconnect();
            SceneManager.LoadScene(3);
        }

        chatClient.Service(); // 포톤 네트워크 서비스 루틴 유지
    }

    // 채팅 UI 요소 동적 생성
    void AddMSG(string sender, object message)
    {
        string msgText = message?.ToString() ?? "";
        ChatItem msg = Instantiate(Resources.Load<ChatItem>("msg"), msgView.transform);
        msg.text = $"{sender} : {msgText}";
        print($"{sender} : {msgText}");
    }

    // 포톤 서버 접속 성공 시 채널 구독
    public void OnConnected()
    {
        chatClient.Subscribe(Session.session.ColorId);
    }

    // 메시지 수신 시 처리
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            AddMSG(senders[i], messages[i]);
        }
    }

    // 접속 해제 콜백
    public void OnDisconnected()
    {
        chatClient.Disconnect();
    }

    // IChatClientListener 인터페이스 나머지 미사용 이벤트
    public void DebugReturn(DebugLevel level, string message) { }
    public void OnChatStateChange(ChatState state) { }
    public void OnPrivateMessage(string sender, object message, string channelName) { }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }
    public void OnSubscribed(string[] channels, bool[] results) { }
    public void OnUnsubscribed(string[] channels) { }
    public void OnUserSubscribed(string channel, string user) { }
    public void OnUserUnsubscribed(string channel, string user) { }
}
