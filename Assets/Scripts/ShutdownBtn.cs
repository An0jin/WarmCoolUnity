using UnityEngine;

public class ShutdownBtn : Btn
{
    //남는 씬이 있지만 종료버튼을 눌렀을떄 만든 클래스
    protected override void OnClick()
    {
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
