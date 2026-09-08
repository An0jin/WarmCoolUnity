using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // Unity UI 시스템 네임스페이스 참조

/// <summary>세션의 진단 결과 중 지정된 항목을 UI 텍스트로 표시합니다.</summary>
public class ResultText : TXT // TXT 기반으로 세션의 결과 텍스트를 자동 업데이트하는 클래스
{
    [SerializeField] ResultType resultType; // 인스펙터에서 텍스트의 종류(ColorId, Cname 등)를 선택하는 필드

    // 부모 클래스 TXT의 SetText 추상 메서드 재정의
    public override void SetText()
    {
        // 설정된 resultType에 맞춰 Session 싱글턴의 데이터값을 Text UI에 할당
        text.text = resultType switch
        {
            ResultType.ColorId => Session.session.ColorId, // 퍼스널 컬러 진단 ID 반환
            ResultType.Cname => Session.session.Cname, // 퍼스널 컬러 이름/립스틱 이름 반환
            _ => "" // 기본값은 빈 문자열
        };
    }
}

/// <summary>결과 화면에서 표시할 데이터의 종류입니다.</summary>
enum ResultType { ColorId, Cname } // 표시할 결과 항목 종류 구분을 위한 열거형
