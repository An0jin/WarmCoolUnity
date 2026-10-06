using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>회원 탈퇴 요청을 서버에 전송하고 로컬 세션을 정리합니다 (Unity 6 Awaitable).</summary>
public class DeleteBtn : MSGBtn
{
    private bool isDelete;

    protected override void Awake()
    {
        isDelete = true;
        base.Awake();
    }

    protected override async void OnClick()
    {
        if (!isDelete) return;
        isDelete = false;

        try
        {
            await APIManager.DeleteAsync($"user/{Session.session.Token}", destroyCancellationToken);
            if (File.Exists(Env.I.Config.FilePath))
            {
                File.Delete(Env.I.Config.FilePath);
            }
            SceneManager.LoadScene(0);
        }
        catch (OperationCanceledException) { }
        catch (Exception)
        {
            Error("삭제 실패. (서버 연결 오류)");
            isDelete = true;
        }
    }
}
