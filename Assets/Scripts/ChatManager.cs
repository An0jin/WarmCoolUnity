using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour
{
    // 채팅 UI 요소
    public InputField inputField;
    public Button sendButton;
    public GameObject messageTemplate;
    public Transform messageContainer;
    public ScrollRect scrollRect;
    
    // 추가적인 설정
    public string playerName = "나";
    public string otherName = "상대방";
    public Color playerMessageColor = new Color(0.8f, 0.9f, 1f);
    public Color otherMessageColor = new Color(1f, 1f, 1f);
    
    // 더미 응답 메시지 목록
    private string[] dummyResponses = new string[] {
        "안녕하세요!",
        "네, 알겠습니다.",
        "그것은 좋은 생각이네요.",
        "정말요? 그렇군요.",
        "다음에 또 연락주세요.",
        "오늘 날씨가 정말 좋네요.",
        "저도 그렇게 생각해요.",
        "조금 더 생각해 볼게요.",
        "아주 흥미로운 이야기네요!",
        "감사합니다!"
    };
    
    // 메시지 저장소
    private List<string> messages = new List<string>();
    
    void Start()
    {
        // 버튼 이벤트 등록
        sendButton.onClick.AddListener(SendMessage);
        
        // 엔터키로 메시지 전송
        inputField.onEndEdit.AddListener(value => {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                SendMessage();
            }
        });
        
        // 초기 메시지 표시
        AddMessage(otherName, "안녕하세요! 무엇을 도와드릴까요?", false);
    }
    
    // 메시지 전송 버튼 클릭 시
    public void SendMessage()
    {
        string message = inputField.text.Trim();
        
        // 빈 메시지가 아닌 경우만 처리
        if (!string.IsNullOrEmpty(message))
        {
            // 플레이어 메시지 추가
            AddMessage(playerName, message, true);
            
            // 입력 필드 초기화
            inputField.text = "";
            inputField.ActivateInputField();
            
            // 응답 생성 (실제 구현에서는 서버나 AI를 통해 응답을 받아야 함)
            Invoke("GenerateResponse", Random.Range(0.5f, 1.5f));
        }
    }
    
    // 더미 응답 생성
    private void GenerateResponse()
    {
        string response = dummyResponses[Random.Range(0, dummyResponses.Length)];
        AddMessage(otherName, response, false);
    }
    
    // 메시지 추가 함수
    public void AddMessage(string sender, string content, bool isPlayerMessage)
    {
        // 메시지 객체 생성
        GameObject messageObj = Instantiate(messageTemplate, messageContainer);
        messageObj.SetActive(true);
        
        // 메시지 텍스트 설정
        Text messageText = messageObj.GetComponentInChildren<Text>();
        messageText.text = $"{sender}: {content}";
        
        // 메시지 배경색 설정
        Image background = messageObj.GetComponent<Image>();
        background.color = isPlayerMessage ? playerMessageColor : otherMessageColor;
        
        // 메시지를 리스트에 추가
        messages.Add($"{sender}: {content}");
        
        // 레이아웃 업데이트를 위해 캔버스 업데이트
        Canvas.ForceUpdateCanvases();
        
        // 스크롤을 아래로 이동
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
    
    // 모든 메시지 지우기
    public void ClearAllMessages()
    {
        foreach (Transform child in messageContainer)
        {
            if (child.gameObject.activeSelf && child.gameObject != messageTemplate)
            {
                Destroy(child.gameObject);
            }
        }
        
        messages.Clear();
    }
}
