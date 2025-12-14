using UnityEngine;
using UnityEngine.UI;

public class Pricy : Btn
{
    protected override void OnClick()
    {
        Application.OpenURL("https://toneiverse.netlify.app/privacy");
    }
}
