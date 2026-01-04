using UnityEngine;
public class BackBtn : Btn
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }
    protected override void OnClick()
    {
        NavigationManager.navigationManager.Back();
    }
}
