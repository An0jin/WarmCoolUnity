using System.Collections; // IEnumerator 코루틴 사용을 위한 System.Collections 참조
using System.Collections.Generic; // List 제네릭 컬렉션 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // 네트워크 관련 참조
using UnityEngine.SceneManagement; // 씬 전환 관리자 참조
using UnityEngine.UI; // UI InputField, Dropdown, Toggle 컴포넌트 참조
using System.IO; // File 클래스를 통한 로컬 파일 저장 참조
using System; // Exception 예외 처리 참조
using Toneiverse; // 프로젝트 열거형(SceneIndex 등) 참조
using Toneiverse.DTO; // DTO 구조체 참조
using Unity.VisualScripting; // VisualScripting 참조

/// <summary>회원가입 입력을 검증하고 계정 생성 및 초기 로그인을 처리합니다.</summary>
public class SignUp : FormBtn // FormBtn 입력 폼 검증 클래스를 상속하는 회원가입 스크립트
{
    [SerializeField] InputField id; // 이메일 계정 ID 입력창
    [SerializeField] Dropdown domain; // 이메일 도메인 선택 드롭다운
    [SerializeField] GetNum numBtn; // 이메일 인증 검증 스크립트 참조
    [SerializeField] Toggle agree; // 약관 동의 토글
    [SerializeField] InputField num; // 사용자가 입력한 인증번호 입력창
    bool isSignUp; // 요청 중복 방지 플래그

    // 초기화 생명주기
    void Awake()
    {
        isSignUp = true;
        base.Awake();
    }

    // 전체 이메일 주소 조합 프로퍼티
    string email => $"{id.text.Trim()}@{domain.options[domain.value].text.ToLower()}";

    // 회원가입 전용 폼 종합 유효성 검사
    protected override bool ValidateForm()
    {
        // 1. 부모 폼 검증 (공백, 나이, 비밀번호 규칙)
        if (!base.ValidateForm())
            return false;

        // 2. 약관 동의 체크 여부 확인
        if (!agree.isOn)
        {
            Error("개인정보처리방침을 동의해주세요.");
            return false;
        }

        // 3. 발급된 인증번호와 입력한 번호 일치 검사
        if (!numBtn.CheckNum(num.text))
        {
            Error("인증번호가 일치하지 않습니다.");
            return false;
        }

        // 4. 인증받은 이메일 주소 변경 여부 확인
        if (!numBtn.CheckEmail(email))
        {
            Error("이메일이 수정되었습니다. 다시 인증해주세요.");
            return false;
        }

        return true;
    }

    // 필수 입력 필드 공백 검사 확장
    protected override bool IsNull()
    {
        return base.IsNull() || string.IsNullOrEmpty(id.text) || string.IsNullOrEmpty(num.text);
    }

    // 가입 버튼 클릭 시 전송 처리
    protected override void OnClick()
    {
        if (isSignUp)
        {
            // 입력 데이터 유효성 판단
            if (!ValidateForm())
            {
                isSignUp = true;
                return;
            }

            Success("회원가입 중...");
            isSignUp = false; // 중복 전송 잠금

            WWWForm form = new WWWForm();
            form.AddField("pw", pw.text);
            form.AddField("name", name.text);
            form.AddField("email", email);
            form.AddField("year", year.text);
            form.AddField("sex", sex);

            // 가입 API 전송
            StartCoroutine(APIManager.Post("user", form, (jsonText) =>
            {
                try
                {
                    SignUpJson json = JsonUtility.FromJson<SignUpJson>(jsonText);
                    if (string.IsNullOrEmpty(json.result)) // 에러 문구가 없으면 회원가입 성공
                    {
                        Token token = new Token();
                        token.token = json.token;
                        File.WriteAllText(Env.I.Config.FilePath, JsonUtility.ToJson(token)); // 로컬 자동로그인 토큰 기록
                        Session.session.SignIn(name.text, email); // 세션 등록
                        SceneManager.LoadScene((int)SceneIndex.Test); // 측정 씬으로 이동
                    }
                    else
                    {
                        Error(json.result);
                        isSignUp = true;
                    }
                }
                catch (Exception e)
                {
                    Error("JSON 파싱 오류: " + e.Message);
                    isSignUp = true;
                }

            }, (error) =>
            {
                Error("웹 요청 오류: " + error);
                isSignUp = true;
            }));
        }
    }
}
