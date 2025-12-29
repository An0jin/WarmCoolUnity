using UnityEngine;

public class ShutdownBtn : Btn
{
    protected override void OnClick()
    {
        print("나가기");
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }
}
