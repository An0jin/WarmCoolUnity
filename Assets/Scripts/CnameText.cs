using UnityEngine;
using UnityEngine.UI;

public class CnameText : TXT
{

    public override void SetText()
    {
        text.text = $"당신의 립스틱 색상 : {Session.session.Cname}";
    }
}
