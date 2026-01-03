using UnityEngine;
using UnityEngine.UI;
using Toneiverse.DTO;
public class GetNum : MSGBtn
{
    private string num, checkEmail;
    [SerializeField] InputField id;
    [SerializeField] Dropdown domain;
    string email => id.text + "@" + domain.options[domain.value].text;
    protected override void OnClick()
    {
        msg.text = "";
        WWWForm form = new WWWForm();
        if (id.text == "")
        {
            Error("이메일을 입력해주세요.");
            return;
        }
        Success("인증번호를 생성하는 중...");
        form.AddField("email", email);
        checkEmail = email;
        num = UnityEngine.Random.Range(0, 9999).ToString("D4");
        form.AddField("num", num);
        StartCoroutine(APIManager.Post("getNum", form, (susses) =>
        {
            Success("인증번호 생성 성공.");
            Json<string> json = JsonUtility.FromJson<Json<string>>(susses);
            Success(json.result);
        }, (error) =>
        {
            Error("인증번호 생성 실패.");
        }));
    }
    public bool CheckNum(string num)
    {
        return num == this.num;
    }

    public bool CheckEmail(string email)
    {
        return email == checkEmail;
    }
}
