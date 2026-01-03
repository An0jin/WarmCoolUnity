using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeleteBtn : MSGBtn
{
    bool isDelete;
    void Awake()
    {
        isDelete = true;
        base.Awake();
    }
    protected override void OnClick()
    {
        if (isDelete)
        {
            isDelete = false;
            StartCoroutine(APIManager.Delete($"user/{Session.session.Token}", (success) =>
            {
                File.Delete(Env.filePath);
                SceneManager.LoadScene(0);

            },
            (err) =>
            {
                Error("삭제 실패. (서버 연결 오류)");
                isDelete = true;

            }));
        }
    }
}
