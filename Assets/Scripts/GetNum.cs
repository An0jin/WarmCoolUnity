using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.UI; // InputField, Dropdown UI 네임스페이스 참조
using Toneiverse.DTO; // DTO 데이터 구조체 참조

/// <summary>이메일 인증번호를 발송하고 가입 시 일치 여부를 확인합니다.</summary>
public class GetNum : MSGBtn // 이메일 인증번호 발송 요청 버튼
{
    private string num, checkEmail; // 생성된 4자리 인증번호 및 인증된 이메일 기록 필드
    [SerializeField] InputField id; // 이메일 ID 입력 필드
    [SerializeField] Dropdown domain; // 이메일 도메인 드롭다운 UI

    // 전체 이메일 주소 조합 프로퍼티
    string email => id.text + "@" + domain.options[domain.value].text;

    // 클릭 시 인증번호 생성 및 요청 로직 실행
    protected override void OnClick()
    {
        msg.text = "";
        WWWForm form = new WWWForm();

        // 아이디 미입력 시 에러 처리
        if (id.text == "")
        {
            Error("이메일을 입력해주세요.");
            return;
        }

        Success("인증번호를 생성하는 중...");
        form.AddField("email", email);
        checkEmail = email;

        // 4자리 난수(0000 ~ 9999) 생성
        num = UnityEngine.Random.Range(0, 9999).ToString("D4");
        form.AddField("num", num);

        // API 통신을 통해 인증번호 발송 요청
        StartCoroutine(APIManager.Post("getNum", form, (jsonText) =>
        {
            Success("인증번호 생성 성공.");
            Json<string> json = JsonUtility.FromJson<Json<string>>(jsonText);
            Success(json.result);
        }, (error) =>
        {
            Error("인증번호 생성 실패.");
        }));
    }

    // 사용자가 입력한 인증번호와 발급된 번호 일치 검사
    public bool CheckNum(string num)
    {
        return num == this.num;
    }

    // 인증 시점의 이메일과 최종 제출 이메일 동일 여부 검사
    public bool CheckEmail(string email)
    {
        return email == checkEmail;
    }
}
