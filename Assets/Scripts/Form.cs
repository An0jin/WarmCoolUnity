using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Toneiverse.DTO;
using Toneiverse;

public class Form : MonoBehaviour
{
    [SerializeField] Button put, delete;
    [SerializeField] InputField name, pw, check;
    bool isUpdate, isDelete;
    [SerializeField] Text msg;
    // Start is called before the first frame update
    void Start()
    {
        isUpdate = true;
        isDelete = true;

        SetInputField(ref name, Session.session.Name);

        put.onClick.AddListener(() =>
        {
            if (isUpdate)
                StartCoroutine(Put());
        });

        delete.onClick.AddListener(() =>
        {
            if (isDelete)
                StartCoroutine(Delete());
        });
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene((int)SceneIndex.Result);
        }
    }
    void SetInputField(ref InputField inputField, object title)
    {
        inputField.text = $"{title}";
    }
    IEnumerator Put()
    {
        string pwPattern = "^[a-zA-Z0-9`~!@#$%^&*()_\\-+=\\[\\]{}|;:'\",<.>/?]{8,16}$";
        msg.color = new Color(1, 1, 1);
        msg.text = "수정중";
        isUpdate = false;
        if (pw.text == "" || name.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "모든정보를 입력해주세요";
            isUpdate = true;
            yield break;//끝내기
        }
        if (!Regex.IsMatch(pw.text, pwPattern))
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "패스워드는 영문과 숫자, 특수문자로 구성되어야 하며 8~16자리여야 합니다.";
            isUpdate = true;
            yield break;
        }
        if (pw.text != check.text)
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "패스워드를 다시 확인해주세요.";
            isUpdate = true;
            yield break;
        }
        UserInfo user = new UserInfo()
        {
            name = name.text,
            pw = pw.text,
            token = Session.session.Token
        };
        using (UnityWebRequest www = UnityWebRequest.Put(Env.Api("user"), JsonUtility.ToJson(user)))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            // 디버그 로그 추가
            Debug.Log("서버 응답 코드: " + www.responseCode);
            Debug.Log("서버 응답 내용: " + www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    Json<string> json = JsonUtility.FromJson<Json<string>>(www.downloadHandler.text);
                    Debug.Log("JSON 파싱 결과: " + JsonUtility.ToJson(json));
                    Session.session.UpdateInfo(name.text);
                    msg.text = json.result;
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                    msg.text = "수정 실패. (응답 처리 오류)";
                    isUpdate = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.text = "수정 실패. (서버 연결 오류)";
                isUpdate = true;
            }
        }
    }
    IEnumerator Delete()
    {
        using (UnityWebRequest www = UnityWebRequest.Delete(Env.Api($"user/{Session.session.Token}")))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            // 디버그 로그 추가
            Debug.Log("서버 응답 코드: " + www.responseCode);
            Debug.Log("서버 응답 내용: " + www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {
                File.Delete(Env.filePath);
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "삭제 실패. (서버 연결 오류)";
                isDelete = true;
            }
        }
    }
}
[Serializable]
public class UserInfo
{
    public string token, pw, name;
}
