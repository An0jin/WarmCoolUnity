using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Toneiverse;
public class LogOutBtn : MonoBehaviour
{
    Button btn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            File.Delete(Env.filePath);
            Session.session.LogOut();
            SceneManager.LoadScene((int)SceneIndex.Title);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
