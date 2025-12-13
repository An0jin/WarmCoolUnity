using UnityEngine;
using UnityEngine.UI;
using Toneiverse.DTO;
public class GetNum : Btn
{
    string num;
    [SerializeField] InputField email;
    [SerializeField] Text msg;
    protected override void OnClick()
    {
        msg.text = "";
        WWWForm form = new WWWForm();
        if (email.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "이메일을 입력해주세요.";
            return;
        }
        msg.color = new Color(1, 1, 1);
        msg.text = "인증번호를 생성하는 중...";
        form.AddField("email", email.text);
        num = UnityEngine.Random.Range(0, 9999).ToString("D4");
        form.AddField("num", num);
        StartCoroutine(APIManager.Post("getNum", form, (susses) =>
        {
            msg.color = new Color(1, 1, 1);
            Json<string> json = JsonUtility.FromJson<Json<string>>(susses);
            msg.text = json.result;
        }, (error) =>
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "인증번호 생성 실패.";
        }));
    }
}
