using UnityEngine;
using UnityEngine.UI;
using Toneiverse.DTO;

public class LLM : MSGBtn
{
    [SerializeField] InputField prompt;
    [SerializeField] Button cls;
    [SerializeField] ResultText cnameText;

    private string originalPlaceholder;
    private Text placeholderComp;

    protected override void Awake()
    {
        base.Awake(); // 필수: 부모의 btn = GetComponent<Button>() 실행
        placeholderComp = prompt.placeholder.GetComponent<Text>();
        originalPlaceholder = placeholderComp.text;
    }

    protected override void OnClick()
    {
        if (string.IsNullOrEmpty(prompt.text)) return;

        SetUIState(false); // UI 잠금

        WWWForm form = new WWWForm();
        form.AddField("token", Session.session.Token);
        form.AddField("msg", prompt.text);
        form.AddField("sex", Session.session.Sex);
        form.AddField("year", Session.session.Year);

        StartCoroutine(APIManager.Post("llm", form,
            (res) =>
            {
                LLMResponse colorJson = JsonUtility.FromJson<LLMResponse>(res);
                Session.session.HexCode = colorJson.hex_code;
                Session.session.Cname = colorJson.cname;
                cnameText.SetText();
                Success(colorJson.result);
                prompt.text = "";
                SetUIState(true);
            },
            (error) =>
            {
                Debug.LogError(error);
                SetUIState(true); // UI 복구
            }
        ));
    }

    // [중복 해결 핵심] UI 상태를 한 번에 제어
    private void SetUIState(bool isReady)
    {
        placeholderComp.text = isReady ? originalPlaceholder : "AI가 생각하고 있습니다.";
        prompt.interactable = isReady;
        btn.interactable = isReady;
        cls.interactable = isReady;
    }
}