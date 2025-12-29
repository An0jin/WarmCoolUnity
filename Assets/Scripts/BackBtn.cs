using UnityEngine;
using UnityEngine.SceneManagement;
using Toneiverse;
public class BackBtn : SceneBtn
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }
}
