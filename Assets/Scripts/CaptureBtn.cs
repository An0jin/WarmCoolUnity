using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Android; // 안드로이드 권한 API 참조
using UnityEngine.UI; // UI Text, Image 컴포넌트 참조

/// <summary>화면 캡처, 로딩 UI 표시, 이미지 전달 과정을 담당하는 기본 클래스입니다 (Unity 6 Awaitable).</summary>
public abstract class CaptureBtn : MSGBtn // 카메라 화면 캡처 및 이미지 바이트 추출 기본 클래스
{
    protected bool canClick = true; // 중복 캡처 방지용 플래그
    protected Text btnText; // 버튼 자식 UI Text
    [SerializeField] protected string lodingMSG; // 캡처 처리 중 로딩 문구
    protected string first_text; // 원본 버튼 텍스트
    [SerializeField] protected BackBtn backBtn; // 뒤로가기 버튼 참조

    // 초기화 생명주기
    protected override void Awake()
    {
        btnText = GetComponentInChildren<Text>();
        first_text = btnText.text;

        // 안드로이드 카메라 권한 확인 및 요청
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        canClick = true;
        base.Awake();
    }

    // 버튼 클릭 이벤트 핸들러
    protected override async void OnClick()
    {
        if (canClick)
        {
            await CaptureAsync();
        }
    }

    // 화면 캡처 수행 비동기 메서드 (Awaitable.EndOfFrameAsync 사용)
    private async Awaitable CaptureAsync()
    {
        canClick = false; // 중복 클릭 차단
        Show(false); // 캡처 중 버튼 UI 숨김

        try
        {
            await Awaitable.EndOfFrameAsync(destroyCancellationToken); // 프레임 렌더링 끝까지 대기

            var img = ScreenCapture.CaptureScreenshotAsTexture(); // 렌더링된 화면을 텍스처로 캡처
            Show(true, lodingMSG); // UI 다시 표시 및 로딩 메시지 표출

            byte[] imgBytes = img.EncodeToJPG();
            Destroy(img); // 캡처 텍스처 메모리 해제

            // 자식 클래스 오버라이드 메서드로 JPG 인코딩 바이너리 전달
            OnCaptureComplete(imgBytes);
        }
        catch (System.OperationCanceledException)
        {
            // 오브젝트 파괴 시 정상 취소 처리
        }
    }

    // 자식 클래스에서 각 기능(진단/CVLLM)에 맞게 백엔드 전송 구현
    protected abstract void OnCaptureComplete(byte[] imgData);

    // UI 제어 가상 메서드
    protected virtual void SetUI(bool value, string label = "")
    {
        msg.text = "";
        btn.interactable = value;
        if (btnText != null) btnText.text = value ? label : "";
    }

    // 캡처 전후 UI 가시성 처리 가상 메서드
    protected virtual void Show(bool value, string result = "")
    {
        print($"result is {result}");
        print($"value is {value}");
        msg.text = "";
        SetBtn(value);
        backBtn.gameObject.SetActive(value);
        btnText.text = result;
    }

    // 버튼 투명도 조절 메서드 (캡처 시 완전히 숨기기 위함)
    protected virtual void SetBtn(bool value)
    {
        var image = btn.GetComponent<Image>();
        if (image != null)
        {
            Color c = image.color;
            c.a = value ? 1f : 0f;
            print($"c.a is {c.a}");
            image.color = c;
        }
    }
}
