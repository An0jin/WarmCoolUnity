using UnityEngine;
using Toneiverse.DTO;
using System;
using UnityEngine.UI;

public class CVLLM : CaptureBtn
{
    [SerializeField] protected Toggle toggle;
    [SerializeField] protected GameObject view;
    protected override void OnCaptureComplete(byte[] img)
    {
        toggle.isOn = false;
        SetBtn(false);
        WWWForm form = new WWWForm();
        form.AddField("color_id", Session.session.ColorId);
        form.AddBinaryData("img", img);
        StartCoroutine(APIManager.Post("cvllm", form, (susses) =>
        {
            try
            {
                print("파일 받음");
                Json<string> json = JsonUtility.FromJson<Json<string>>(susses);
                toggle.isOn = true;
                print(json.result);
                Show(true, first_text);
                Success(json.result);
            }
            catch (Exception e)
            {
                Error("JSON 파싱 오류: " + e.Message);
            }
            SetBtn(true);

        }, (error) =>
        {
            Error("웹 요청 오류: " + error);
            Show(true, first_text);

        }));
        canClick = true;

    }
    protected override void Show(bool value, string result = "")
    {
        view.SetActive(value);
        base.Show(value, result);
    }
}
