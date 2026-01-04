using System.Collections;
using System.Collections.Generic;
using Toneiverse;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class SceneBtn : Btn
{
    [SerializeField] protected SceneIndex scene;
    protected override void OnClick()
    {
        NavigationManager.navigationManager.Front(scene);
    }
}
