using UnityEngine;
using UnityEngine.UI;

public class ResultText : TXT
{
    public override void SetText()
    {
        text.text = $"당신의 퍼스널컬러: {Session.session.ColorId}";
    }
}
