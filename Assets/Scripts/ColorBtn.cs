using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class ColorBtn : Btn
{
    private string hex, cname;
    public void SetBtnColor(string hex, string cname)
    {
        this.hex = hex;
        this.cname = cname;
        ColorBlock colors = btn.colors;
        Color tmp;
        ColorUtility.TryParseHtmlString(hex, out tmp);
        colors.normalColor = tmp;
        btn.colors = colors;
    }

    protected override void OnClick()
    {
        Session.session.HexCode = hex;
        print(cname);
        Session.session.Cname = cname;
        Lipstick lipstick = new Lipstick()
        {
            token = Session.session.Token,
            hex_code = Session.session.HexCode
        };
        string json = JsonUtility.ToJson(lipstick);
        StartCoroutine(APIManager.Put("user/lipstick", json));

    }
}
[Serializable]
public class Lipstick
{
    public string token, hex_code;
}