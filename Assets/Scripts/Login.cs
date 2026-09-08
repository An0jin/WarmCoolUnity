using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using Toneiverse.DTO; // DTO 구조체 참조
using UnityEngine.UI; // InputField, Text UI 참조
using System.IO; // 로컬 파일 입출력 참조
using UnityEngine.SceneManagement; // 씬 관리 참조
using Toneiverse; // SceneIndex 열거형 참조
using System; // Exception 참조

/// <summary>로그인 요청을 처리하고 발급된 토큰을 로컬에 저장합니다.</summary>
public class Login : Btn // 로그인 버튼 기능을 담당하는 스크립트
{
    [SerializeField] InputField email, pw; // 이메일 및 비밀번호 입력 UI 필드
    [SerializeField] Text msg; // 결과 상태 텍스트 필드

    // 버튼 클릭 핸들러
    protected override void OnClick()
    {
        msg.color = new Color(1, 1, 1); // 텍스트 색상을 흰색으로 설정
        msg.text = "로그인 중...";

        // 필수 필드 입력 검사
        if (email.text == "" || pw.text == "")
        {
            msg.color = new Color(1, 0, 0); // 에러 표시(빨간색)
            msg.text = "이메일과 비밀번호를 입력해주세요.";
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("pw", pw.text);

        // 로그인 API POST 요청
        StartCoroutine(APIManager.Post("login", form, (jsonText) =>
        {
            try
            {
                InfoJson json = JsonUtility.FromJson<InfoJson>(jsonText);
                if (json.msg == "성공")
                {
                    Token token = new Token();
                    token.token = json.token;
                    File.WriteAllText(Env.I.Config.FilePath, JsonUtility.ToJson(token)); // 토큰 파일 동기화
                    Session.session.Login(json); // 사용자 데이터 세션 바인딩
                    
                    // 세션 데이터 완성도에 맞춰 적절한 진입 씬으로 이동
                    NavigationManager.navigationManager.Front(
                        string.IsNullOrEmpty(Session.session.Sex) ? SceneIndex.ProfileSetup : 
                        string.IsNullOrEmpty(Session.session.HexCode) ? SceneIndex.Test : 
                        SceneIndex.Result
                    );
                }
                else
                {
                    msg.text = json.msg;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("JSON 파싱 오류: " + e.Message);
                msg.color = new Color(1, 0, 0);
                msg.text = "로그인 실패. (응답 처리 오류)";
            }
        }));
    }
}
