using UnityEngine;
using UnityEngine.UI;

public class ResultText : TXT
{
    [SerializeField] ResultType resultType;
    public override void SetText()
    {

        text.text = resultType switch
        {
            ResultType.ColorId => Session.session.ColorId,
            ResultType.Cname => Session.session.Cname,
            _ => ""
        };
    }
}
enum ResultType { ColorId, Cname }
