using UnityEngine;
using UnityEngine.UI;

public abstract class TXT : MonoBehaviour
{
    protected Text text;

    protected virtual void Awake()
    {
        text = GetComponent<Text>();
        SetText();

    }
    public abstract void SetText();
}
