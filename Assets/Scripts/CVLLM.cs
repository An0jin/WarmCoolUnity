using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // IMultipartFormSection 등 네트워킹 참조
using System.Collections.Generic; // List 제네릭 컬렉션 참조
using Toneiverse.DTO; // DTO 데이터 구조체 참조
using System; // Exception 예외 처리 참조
using UnityEngine.UI; // Toggle UI 컴포넌트 참조

/// <summary>캡처 이미지를 분석 API로 전송하고 생성된 결과를 표시합니다 (Unity 6 Awaitable).</summary>
public class CVLLM : CaptureBtn // 화면 캡처 기반 Vision LLM 이미지 분석 스크립트
{
    [SerializeField] protected Toggle toggle; // 결과 표시 토글
    [SerializeField] protected GameObject view; // 결과 뷰 패널

    // CaptureBtn에서 캡처한 이미지 바이트 전송 콜백 (Awaitable 비동기 처리)
    protected override async void OnCaptureComplete(byte[] img)
    {
        toggle.isOn = false;
        view.SetActive(false);
        toggle.gameObject.SetActive(false);

        List<IMultipartFormSection> form = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("color_id", Session.session.ColorId), // 유저 ColorId 전달
            new MultipartFormFileSection("img", img, "capture.jpg", "image/jpeg") // 캡처 이미지 전달
        };

        try
        {
            // 백엔드 "cvllm" API Awaitable 요청
            var json = await APIManager.PostJsonAsync<Json<string>>("cvllm", form, destroyCancellationToken);
            print("파일 받음");
            view.SetActive(true);
            toggle.gameObject.SetActive(true);
            toggle.isOn = true;
            Show(true, first_text);
            Success(json.result); // LLM 텍스트 결과 표시
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            Error("웹 요청 오류: " + e.Message);
            Show(true, first_text);
        }
        finally
        {
            canClick = true;
        }
    }
}
