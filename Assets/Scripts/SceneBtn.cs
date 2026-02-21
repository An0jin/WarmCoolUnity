using Toneiverse;
using UnityEngine;

public class SceneBtn : Btn
{
    [SerializeField] protected SceneIndex scene;
    protected override void OnClick()
    {
        NavigationManager.navigationManager.Front(scene);
    }
}
