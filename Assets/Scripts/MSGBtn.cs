using UnityEngine;
using UnityEngine.UI;
public abstract class MSGBtn : Btn
{
    [SerializeField] protected Text msg;
    private void SetMSG(Color color, string text)
    {
        msg.color = color;
        msg.text = text;
    }
    protected virtual void Success(string text)
    {
        SetMSG(new Color(248, 149, 114), text);
    }
    protected virtual void Error(string text)
    {
        SetMSG(Color.red, text);
    }
}
