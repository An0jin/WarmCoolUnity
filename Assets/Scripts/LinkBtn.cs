using UnityEngine;

public class LinkBtn : Btn
{
    protected override void OnClick()
    {
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
    }
    [field: SerializeField] public string url { set; private get; }
}
