using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public abstract class Btn : MonoBehaviour
{
    protected Button btn;

    protected virtual void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    protected abstract void OnClick();
}