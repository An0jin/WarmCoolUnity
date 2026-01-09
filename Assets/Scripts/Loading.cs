using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Toneiverse.DTO;
using Toneiverse;
using System;

public class Loading : MonoBehaviour
{
    [SerializeField] Text msg;
    [SerializeField] GameObject susses;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckVersion();
        SetLoading(true);
    }

    void SetLoading(bool show)
    {
        susses.SetActive(!show);
        msg.gameObject.SetActive(show);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void CheckAutoLogin()
    {
        msg.text = "자동 로그인을 체크하는중...";
        //파일 시스템이 존재하는가?
        if (File.Exists(Env.filePath))
        {
            string data = File.ReadAllText(Env.filePath);
            if (string.IsNullOrEmpty(data))
            {
                SetLoading(false);
                return;
            }
            Token token = JsonUtility.FromJson<Token>(data);
            print("token : " + token.token);
            if (string.IsNullOrEmpty(token.token))
            {
                SetLoading(false);
                return;
            }
            else
            {
                StartCoroutine(APIManager.Get($"/user/{token.token}", (jsonText) =>
                {
                    InfoJson json = JsonUtility.FromJson<InfoJson>(jsonText);
                    print("이메일 : " + json.email);
                    if (string.IsNullOrEmpty(json.email))
                    {
                        SetLoading(false);
                        File.Delete(Env.filePath);
                    }
                    else
                    {
                        Session.session.Login(json);
                        NavigationManager.navigationManager.Front(string.IsNullOrEmpty(Session.session.Sex) ? SceneIndex.ProfileSetup : string.IsNullOrEmpty(Session.session.HexCode) ? SceneIndex.Test : SceneIndex.Result);
                    }
                }));
            }
        }
        else
        {
            SetLoading(false);
        }
    }

    void CheckVersion()
    {
        msg.text = "버전 체크중...";

        StartCoroutine(APIManager.Get($"/version/{Application.version}", (jsonText) =>
        {
            if (string.IsNullOrEmpty(jsonText))
            {
                Debug.LogError("Server Response is Empty");
                msg.text = "서버 점검중이거나 서버에 문제가 생겼습니다";
                return;
            }

            Json<bool> json = JsonUtility.FromJson<Json<bool>>(jsonText);

            // 3. 파싱 결과가 null인지 확인
            if (json == null)
            {
                throw new Exception("JSON Parsing returned null");
            }

            if (json.result)
            {
                CheckAutoLogin();
            }
            else
            {
                string url = "";
#if UNITY_IOS
                url = "나중에 만들예정";
#else
                url = "https://play.google.com/store/apps/details?id=com.an0jin.Toneiverse";
#endif
                Application.OpenURL(url);
                msg.text = "업데이트 필요";
                Application.Quit();
            }
        }));
    }
}
