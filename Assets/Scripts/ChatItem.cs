using System.Collections; // System.Collections 참조
using System.Collections.Generic; // System.Collections.Generic 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // Unity UI Text 컴포넌트 참조

/// <summary>채팅 메시지 한 줄의 텍스트와 작성자 표시 색상을 관리합니다.</summary>
public class ChatItem : MonoBehaviour // 채팅 목록의 개별 항목 프리팹 제어 클래스
{
    Text msg; // 메시지 텍스트 UI 컴포넌트
    string _email; // 작성자 이메일

    // 컴포넌트 초기화
    void Awake()
    {
        msg = GetComponent<Text>(); // 컴포넌트 가져오기
    }

    // 메시지 텍스트 갱신 프로퍼티
    public string text
    {
        set => msg.text = value; // 메시지 문자열 입력
    }

    /// <summary>
    /// Sets the ID and updates the text color based on whether the session's user ID matches the provided ID.
    /// </summary>
    // 작성자 이메일 설정 및 내 메시지 여부에 따른 색상(빨간색/검은색) 분류 프로퍼티
    public string email
    {
        set
        {
            _email = value;
            // 본인 메시지면 빨간색 계열(R=1), 타인 메시지면 검은색 계열(R=0) 적용
            msg.color = new Color(Session.session.Email == _email ? 1 : 0, 0, 0);
        }
    }
}
