using System.Collections.Generic; // 제네릭 컬렉션 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // 네트워크 참조
using UnityEngine.SceneManagement; // 씬 전환 참조
using Toneiverse.DTO; // DTO 객체 참조
using Toneiverse; // SceneIndex 참조
using System; // OperationCanceledException 참조

/// <summary>캡처 이미지를 진단 API로 보내고 퍼스널 컬러 결과를 적용하며 결과 씬으로 이동합니다 (Unity 6 Awaitable).</summary>
public class Test : CaptureBtn // 화면 캡처 기반 퍼스널컬러 진단 측정 스크립트
{
    // 캡처 완료 시 호출되는 이미지 처리 메서드 구현 (Awaitable)
    protected override async void OnCaptureComplete(byte[] img)
    {
        List<IMultipartFormSection> form = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("token", Session.session.Token), // 유저 인증 토큰
            new MultipartFormFileSection("img", img, "capture.jpg", "image/jpeg") // 캡처 이미지 데이터
        };

        try
        {
            // 백엔드 "predict" 퍼스널컬러 분석 API 전송 (Awaitable)
            ColorJson colorJson = await APIManager.PostJsonAsync<ColorJson>("predict", form, destroyCancellationToken);

            // 진단 실패 시
            if (string.IsNullOrEmpty(colorJson.cname))
            {
                Show(true, first_text);
                Error(colorJson.color_id); // 에러 메시지 표출
                canClick = true;
            }
            else
            {
                Session.session.Predict(colorJson); // 세션에 결과 반영
                SceneManager.LoadScene((int)SceneIndex.Result); // 결과 화면 씬으로 이동
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            Debug.LogError($"[Test] 진단 요청 오류: {e.Message}");
            Show(true, first_text);
            Error("서버 분석 오류가 발생했습니다.");
            canClick = true;
        }
    }
}
