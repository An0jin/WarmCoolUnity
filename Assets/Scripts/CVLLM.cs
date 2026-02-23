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
        view.SetActive(false);
        toggle.gameObject.SetActive(false);
        WWWForm form = new WWWForm();
        form.AddField("color_id", Session.session.ColorId);
        form.AddBinaryData("img", img);
        StartCoroutine(APIManager.Post("cvllm", form, (susses) =>
        {
            try
            {
                print("파일 받음");
                view.SetActive(true);
                toggle.gameObject.SetActive(true);
                Json<string> json = JsonUtility.FromJson<Json<string>>(susses);
                toggle.isOn = true;
                Show(true, first_text);
                print(json.result);
                Success(json.result);
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
