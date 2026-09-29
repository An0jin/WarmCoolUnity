using System.Collections; // IEnumerator 코루틴 참조
using System.Collections.Generic; // 제네릭 컬렉션 참조
using System.Threading.Tasks; // Task 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Events; // UnityEvent 참조
using UnityEngine.Android; // 안드로이드 참조
using UnityEngine.Networking; // 네트워크 참조
using UnityEngine.SceneManagement; // 씬 전환 참조
using UnityEngine.UI; // UI 참조
using Toneiverse.DTO; // DTO 객체 참조
using Toneiverse; // SceneIndex 참조

/// <summary>캡처 이미지를 진단 API로 보내고 퍼스널 컬러 결과를 적용하며 인스펙터 진단 이벤트를 발송합니다.</summary>
public class Test : CaptureBtn // 화면 캡처 기반 퍼스널컬러 진단 측정 스크립트
{
    [Header("진단 이벤트 설정")]
    [SerializeField] public UnityEvent<ColorJson> onDiagnosisSuccessEvent = new UnityEvent<ColorJson>();
    [SerializeField] public UnityEvent<string> onDiagnosisFailedEvent = new UnityEvent<string>();

    // 캡처 완료 시 호출되는 이미지 처리 메서드 구현
    protected override void OnCaptureComplete(byte[] img)
    {
        WWWForm form = new WWWForm();
        form.AddField("token", Session.session.Token); // 유저 인증 토큰
        form.AddBinaryData("img", img); // 캡처 이미지 데이터

        // 백엔드 "predict" 퍼스널컬러 분석 API 전송
        StartCoroutine(APIManager.Post("predict", form, (jsonText) =>
        {
            ColorJson colorJson = JsonUtility.FromJson<ColorJson>(jsonText);
            
            // 진단 실패 시
            if (string.IsNullOrEmpty(colorJson.cname))
            {
                Show(true, first_text);
                Error(colorJson.color_id); // 에러 메시지 표출
                onDiagnosisFailedEvent?.Invoke(colorJson.color_id); // 진단 실패 Unity 이벤트 통지
                canClick = true;
            }
            else
            {
                Session.session.Predict(colorJson); // 세션에 결과 반영
                onDiagnosisSuccessEvent?.Invoke(colorJson); // 진단 성공 Unity 이벤트 통지
                SceneManager.LoadScene((int)SceneIndex.Result); // 결과 화면 씬으로 이동
            }
        }));
    }
}
