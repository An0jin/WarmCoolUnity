using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Toneiverse;
public class LogOutBtn : SceneBtn
{
    protected override void OnClick()
    {
        File.Delete(Env.I.Config.FilePath);
        Session.session.LogOut();
        base.OnClick();
    }
}
