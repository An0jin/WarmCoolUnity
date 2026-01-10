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
        Color color = new Color(250 / 255f, 156 / 255f, 120 / 255f);
        SetMSG(color, text);
    }
    protected virtual void Error(string text)
    {
        Color color = Color.red;
        SetMSG(color, text);
    }
}
