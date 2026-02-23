using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public abstract class CaptureBtn : MSGBtn
{
    protected bool canClick = true;
    protected Text btnText;
    [SerializeField] protected string lodingMSG;
    protected string first_text;
    [SerializeField] protected BackBtn backBtn;

    protected override void Awake()
    {
        btnText = GetComponentInChildren<Text>();
        first_text = btnText.text;
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {

            Permission.RequestUserPermission(Permission.Camera);
        }
        canClick = true;
        base.Awake();
    }

    protected override void OnClick()
    {
        if (canClick) StartCoroutine(Capture());
    }

    private IEnumerator Capture()
    {
        canClick = false;
        Show(false);
        yield return new WaitForEndOfFrame();

        var img = ScreenCapture.CaptureScreenshotAsTexture();
        Show(true, lodingMSG);

        // 실제 서버 통신 로직은 자식 클래스에서 구현하도록 넘김
        OnCaptureComplete(img.EncodeToJPG());
    }

    // 자식마다 서버 엔드포인트나 처리 로직이 다르므로 추상 메서드로 선언
    protected abstract void OnCaptureComplete(byte[] imgData);

    // UI 상태 제어 공통 로직
    protected virtual void SetUI(bool value, string label = "")
    {
        msg.text = "";
        btn.interactable = value;
        if (btnText != null) btnText.text = value ? label : "";
    }

    protected virtual void Show(bool value, string result = "")
    {
        print($"result is {result}");
        print($"value is {value}");
        msg.text = "";
        SetBtn(value);
        backBtn.gameObject.SetActive(value);
        btnText.text = result;
    }
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
