using System.Collections; // IEnumerator 코루틴 사용을 위한 System.Collections 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // 네트워크 관련 참조
using UnityEngine.UI; // Button, ColorBlock 등 UI 시스템 참조
using System; // Action 및 System 네임스페이스 참조
using Toneiverse.DTO; // DTO 객체 참조

/// <summary>립스틱 색상을 표시하고 선택값을 세션과 서버에 저장합니다.</summary>
public class ColorBtn : Btn // 립스틱 대표 색상을 선택하는 버튼 구현 클래스
{
    private string hex, cname; // 색상 16진수 코드(HEX) 및 립스틱 명칭 필드
    ResultText cnameText; // 결과 화면 텍스트 업데이트용 참조 변수

    // 동적으로 생성된 버튼의 색상과 데이터를 바인딩하는 설정 함수
    public void SetBtnColor(string hex, string cname, ResultText cnameText)
    {
        this.cnameText = cnameText; // 텍스트 표시 컴포넌트 저장
        this.hex = hex; // HEX 코드 데이터 저장
        this.cname = cname; // 립스틱 명칭 데이터 저장
        ColorBlock colors = btn.colors; // UI Button의 색상 블록 가져오기
        Color tmp; // 파싱 결과를 받아올 Color 변수
        ColorUtility.TryParseHtmlString(hex, out tmp); // HEX 문자열(#RRGGBB)을 Color 구조체로 파싱
        colors.normalColor = tmp; // 일반 상태 색상에 파싱한 컬러 지정
        btn.colors = colors; // 버튼 컴포넌트에 색상 적용
    }

    // 립스틱 버튼 클릭 이벤트 핸들러
    protected override void OnClick()
    {
        Session.session.HexCode = hex; // 세션에 HEX 코드 할당 (AR 착색 변경 이벤트 발동)
        Session.session.Cname = cname; // 세션에 립스틱 명칭 저장
        cnameText.SetText(); // UI 텍스트 동기화 업데이트

        // 서버 업데이트용 Lipstick DTO 구성
        Lipstick lipstick = new Lipstick()
        {
            token = Session.session.Token, // 인증 토큰
            hex_code = Session.session.HexCode // 선택된 HEX 코드
        };
        string json = JsonUtility.ToJson(lipstick); // JSON 문자열 변환
        StartCoroutine(APIManager.Put("user/lipstick", json)); // 백엔드 서버에 대표 립스틱 설정 전송
    }
}
