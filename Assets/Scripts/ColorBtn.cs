using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using Toneiverse.DTO;

public class ColorBtn : Btn
{
    private string hex, cname;
    ResultText cnameText;
    public void SetBtnColor(string hex, string cname, ResultText cnameText)
    {
        this.cnameText = cnameText;
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
        Session.session.Cname = cname;
        cnameText.SetText();
        Lipstick lipstick = new Lipstick()
        {
            token = Session.session.Token,
            hex_code = Session.session.HexCode
        };
        string json = JsonUtility.ToJson(lipstick);
        StartCoroutine(APIManager.Put("user/lipstick", json));
    }
}