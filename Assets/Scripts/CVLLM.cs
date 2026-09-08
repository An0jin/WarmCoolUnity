using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using Toneiverse.DTO; // DTO 데이터 구조체 참조
using System; // Exception 예외 처리 참조
using UnityEngine.UI; // Toggle UI 컴포넌트 참조

/// <summary>캡처 이미지를 분석 API로 전송하고 생성된 결과를 표시합니다.</summary>
public class CVLLM : CaptureBtn // 화면 캡처 기반 Vision LLM 이미지 분석 스크립트
{
    [SerializeField] protected Toggle toggle; // 결과 표시 토글
    [SerializeField] protected GameObject view; // 결과 뷰 패널

    // CaptureBtn에서 캡처한 이미지 바이트 전송 콜백
    protected override void OnCaptureComplete(byte[] img)
    {
        toggle.isOn = false;
        view.SetActive(false);
        toggle.gameObject.SetActive(false);

        WWWForm form = new WWWForm();
        form.AddField("color_id", Session.session.ColorId); // 유저 ColorId 전달
        form.AddBinaryData("img", img); // 캡처 이미지 전달

        // 백엔드 "cvllm" API 요청
        StartCoroutine(APIManager.Post("cvllm", form, (jsonText) =>
        {
            try
            {
                print("파일 받음");
                view.SetActive(true);
                toggle.gameObject.SetActive(true);
                Json<string> json = JsonUtility.FromJson<Json<string>>(jsonText);
                toggle.isOn = true;
                Show(true, first_text);
                print(json.result);
                Success(json.result); // LLM 텍스트 결과 표시
            }
            catch (Exception e)
            {
                Error("JSON 파싱 오류: " + e.Message);
            }

        }, (error) =>
        {
            Error("웹 요청 오류: " + error);
            Show(true, first_text);
        }));
        canClick = true;
    }
}
