using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // IMultipartFormSection 등 네트워킹 참조
using System.Collections.Generic; // List 제네릭 컬렉션 참조
using Toneiverse.DTO; // DTO 구조체 참조
using UnityEngine.UI; // InputField, Text UI 참조
using System.IO; // 로컬 파일 입출력 참조
using UnityEngine.SceneManagement; // 씬 관리 참조
using Toneiverse; // SceneIndex 열거형 참조
using System; // Exception 참조

/// <summary>로그인 요청을 처리하고 발급된 토큰을 로컬에 저장하며 다음 씬으로 이동합니다.</summary>
public class Login : Btn // 로그인 버튼 기능을 담당하는 스크립트
{
    [SerializeField] InputField email, pw; // 이메일 및 비밀번호 입력 UI 필드
    [SerializeField] Text msg; // 결과 상태 텍스트 필드

    // 버튼 클릭 핸들러 (Unity 6 Awaitable 비동기 패턴)
    protected override async void OnClick()
    {
        msg.color = new Color(1, 1, 1); // 텍스트 색상을 흰색으로 설정
        msg.text = "로그인 중...";

        // 필수 필드 입력 검사
        if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(pw.text))
        {
            msg.color = new Color(1, 0, 0); // 에러 표시(빨간색)
            msg.text = "이메일과 비밀번호를 입력해주세요.";
            return;
        }

        btn.interactable = false; // 중복 클릭 방지

        try
        {
            List<IMultipartFormSection> form = new List<IMultipartFormSection>
            {
                new MultipartFormDataSection("email", email.text),
                new MultipartFormDataSection("pw", pw.text)
            };

            // APIManager의 Awaitable JSON 역직렬화 메서드 호출 (오브젝트 파괴 시 자동 취소)
            InfoJson json = await APIManager.PostJsonAsync<InfoJson>("login", form, destroyCancellationToken);

            if (json != null && json.msg == "성공")
            {
                Token token = new Token { token = json.token };
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
                msg.color = new Color(1, 0, 0);
                msg.text = json != null ? json.msg : "서버 응답이 올바르지 않습니다.";
            }
        }
        catch (OperationCanceledException)
        {
            // 오브젝트 파괴 또는 씬 전환 시 정상 취소 처리
        }
        catch (Exception e)
        {
            Debug.LogError($"[Login] 오류 발생: {e.Message}");
            msg.color = new Color(1, 0, 0);
            msg.text = "로그인 실패. (서버 응답 오류)";
        }
        finally
        {
            if (btn != null) btn.interactable = true; // 버튼 상태 복구
        }
    }
}
