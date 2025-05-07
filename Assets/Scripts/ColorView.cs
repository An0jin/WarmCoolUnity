using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorView : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SetColorView());
    }

    IEnumerator SetColorView(){
        using(UnityWebRequest www=UnityWebRequest.Get(Env.Api($"/lipstick/{Session.session.ColorId}"))){
            yield return www.SendWebRequest();
            if(www.result==UnityWebRequest.Result.Success){
                JsonList<string> json=JsonUtility.FromJson<JsonList<string>>(www.downloadHandler.text);
                foreach (var item in json.result)
                {
                    ColorBtn btn=Instantiate(Resources.Load<ColorBtn>("ColorBtn"),transform);
                    btn.color=item;
                }
            }
        }
    }
}
