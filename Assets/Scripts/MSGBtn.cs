using UnityEngine;
using UnityEngine.UI;
public abstract class MSGBtn : Btn
{
    [SerializeField] protected Text msg;
    [SerializeField] protected Color color = new Color(250 / 255f, 156 / 255f, 120 / 255f);
    private void SetMSG(string text, Color color)
    {
        msg.color = color;
        msg.text = text;
    }
    protected virtual void Success(string text)
    {
        SetMSG(text, color);
    }
    protected virtual void Error(string text)
    {
        SetMSG(text, Color.red);
    }
}
