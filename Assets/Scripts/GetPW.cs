using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Toneiverse.DTO;

public class GetPW : Btn
{
    [SerializeField] InputField email;
    [SerializeField] Text msg;

    protected override void OnClick()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        msg.text = "아이디와 비밀번호 찾는중...";
        StartCoroutine(APIManager.Post("email", form, (data) =>
        {
            Json<string> result = JsonUtility.FromJson<Json<string>>(data);
            msg.text = result.result;
        }, (error) =>
        {
            msg.text = "로그인 실패. (서버 연결 오류)";
        }));
    }
}