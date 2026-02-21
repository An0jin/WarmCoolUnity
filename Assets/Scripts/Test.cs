using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Toneiverse.DTO;
using Toneiverse;

public class Test : CaptureBtn
{
    protected override void OnCaptureComplete(byte[] img)
    {
        WWWForm form = new WWWForm();
        form.AddField("token", Session.session.Token);
        form.AddBinaryData("img", img);
        StartCoroutine(APIManager.Post("predict", form, (jsonText) =>
        {
            ColorJson colorJson = JsonUtility.FromJson<ColorJson>(jsonText);
            if (string.IsNullOrEmpty(colorJson.cname))
            {
                Show(true, first_text);
                Error(colorJson.color_id);
                canClick = true;
            }
            else
            {
                Session.session.Predict(colorJson);
                SceneManager.LoadScene((int)SceneIndex.Result);
            }
        }));
    }
}
