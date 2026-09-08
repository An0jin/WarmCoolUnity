using System; // DateTime 등 닷넷 기본 클래스 참조
using System.Text.RegularExpressions; // 정규 표현식(Regex) 네임스페이스 참조
using Unity.VisualScripting; // Unity Visual Scripting 네임스페이스 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // InputField, Toggle UI 컴포넌트 참조

/// <summary>회원가입과 정보 수정 폼의 공통 입력값 검증 기능을 제공합니다.</summary>
public abstract class FormBtn : MSGBtn // 폼 입력 요소의 유효성 검증을 담당하는 추상 기반 클래스
{
    [SerializeField] protected InputField pw, pwConfirm, name, year; // 비밀번호, 확인비밀번호, 이름, 출생연도 입력 필드
    [SerializeField] protected Toggle man; // 남성 선택 토글

    // 성별 텍스트 변환 프로퍼티 (남자 토글 켜짐 여부에 따라 구분)
    protected string sex => man.isOn ? "남자" : "여자";

    // 비밀번호 정규 표현식 검증 메서드 (영문/숫자/특수문자 8~16자리)
    protected virtual bool MatchPw(string pw)
    {
        string pwPattern = @"^[a-zA-Z0-9`~!@#$%^&*()_+=\[\]{}|;:'"",.<>/?-]{8,16}$";
        if (string.IsNullOrEmpty(pw))
            return false;
        return Regex.IsMatch(pw, pwPattern);
    }

    // 비밀번호와 비밀번호 확인 텍스트 동일 여부 검사
    protected virtual bool CheckPw => pw.text == pwConfirm.text;

    // 입력 필드 누락(공백) 여부 체크
    protected virtual bool IsNull()
    {
        return string.IsNullOrEmpty(pw.text) || string.IsNullOrEmpty(pwConfirm.text) || string.IsNullOrEmpty(name.text);
    }

    // 폼 입력값의 전체 유효성 검증 수행 메서드
    protected virtual bool ValidateForm()
    {
        // 1. 필수 입력 필드 공백 검사
        if (IsNull())
        {
            Error("모든 정보를 입력해주세요.");
            return false;
        }

        // 2. 출생연도 수치 및 나이 범위 검사 (1~120세)
        int currentYear = DateTime.Now.Year;
        if (!int.TryParse(year.text, out int birth) || currentYear - birth < 1 || currentYear - birth > 120)
        {
            Error("태어난 연도가 이상합니다");
            return false;
        }

        // 3. 비밀번호 자릿수 및 조합 패턴 검사
        if (!MatchPw(pw.text))
        {
            Error("비밀번호는 영문과 숫자, 특수문자로 구성되어야 하며 8~16자리여야 합니다.");
            return false;
        }

        // 4. 비밀번호 2차 재확인 검사
        if (!CheckPw)
        {
            Error("비밀번호가 일치하지 않습니다.");
            return false;
        }

        return true; // 모든 입력값이 유효함을 의미
    }
}
