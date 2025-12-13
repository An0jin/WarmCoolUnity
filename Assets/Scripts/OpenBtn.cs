using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class OpenBtn : Btn
{
    [SerializeField] GameObject obj;

    protected override void OnClick()
    {
        obj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
