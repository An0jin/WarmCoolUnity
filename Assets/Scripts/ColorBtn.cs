using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class ColorBtn : MonoBehaviour
{
    Button btn;
    private string _color;
    public string color
    {
        set
        {
            _color = value;
            ColorBlock colors = btn.colors;
            Color tmp;
            ColorUtility.TryParseHtmlString(value, out tmp);
            colors.normalColor = tmp;
            btn.colors = colors;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            StartCoroutine(UpdateLipstick());
        });
    }
    IEnumerator UpdateLipstick()
    {
        Session.session.HexCode = _color;
        Lipstick lipstick=new Lipstick();
        lipstick.user_id=Session.session.UserId;
        lipstick.hex_code=Session.session.HexCode;
        string json=JsonUtility.ToJson(lipstick);
        using(UnityWebRequest www=UnityWebRequest.Put(Env.Api("user/lipstick"),json)){
            www.SetRequestHeader("Content-Type","application/json");
            yield return www.SendWebRequest();
            
        }

    }
}
[Serializable]
public class Lipstick{
    public string user_id,hex_code;
}