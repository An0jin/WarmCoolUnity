using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SignUp : MonoBehaviour
{
    Button signUP;
    InputField id, name, year, pw;
    bool isSignUp;
    Toggle man;
    Text msg;
    // Start is called before the first frame update
    void Start()
    {
        isSignUp = true;
        id = GameObject.Find("id").GetComponent<InputField>();
        name = GameObject.Find("name").GetComponent<InputField>();
        year = GameObject.Find("year").GetComponent<InputField>();
        pw = GameObject.Find("pw").GetComponent<InputField>();
        man = GameObject.Find("man").GetComponent<Toggle>();
        signUP = GameObject.Find("SignUP").GetComponent<Button>();
        msg = GameObject.Find("msg").GetComponent<Text>();
        signUP.onClick.AddListener(() =>
        {
            if (isSignUp)
                StartCoroutine(SignUP());
        });
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {    
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(0);
        }
    
    }
    IEnumerator SignUP()
    {
        string idPattern = "^[a-zA-Z0-9]{8,16}$";
        string pwPattern = "^[a-zA-Z0-9`~!@#$%^&*()_\\-+=\\[\\]{}|;:'\",<.>/?]{8,16}$";
        string gender = man.isOn ? "Male" : "Female";
        int iYear=int.Parse(year.text);
        msg.color = new Color(1, 1, 1);
        msg.text = "Signing up...";
        isSignUp = false;
        if (id.text == "" || pw.text == "" || year.text == "" || name.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "Please enter all information";
            isSignUp = true;
            yield break;//끝내기
        }
        int today = DateTime.Today.Year;
        if (int.Parse(year.text) > today || int.Parse(year.text) < today - 100)
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "Birth year is not valid";
            isSignUp = true;
            yield break;//끝내기
        }
        if (!Regex.IsMatch(id.text, idPattern))
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "The ID must be composed of only English letters and numbers and must be between 8 and 16 characters long.";
            isSignUp = true;
            yield break;
        }
        if (!Regex.IsMatch(pw.text, pwPattern))
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "The password must be composed of only English letters and numbers and special characters and must be between 8 and 16 characters long.";
            isSignUp = true;
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", id.text);
        Debug.Log($"id : {id.text}");
        form.AddField("pw", pw.text);
        Debug.Log($"pw : {pw.text}");
        form.AddField("name", name.text);
        Debug.Log($"name : {name.text}");
        form.AddField("year",iYear);
        Debug.Log($"year : {iYear}");
        form.AddField("gender", gender);
        Debug.Log($"gender : {gender}");
        using (UnityWebRequest www = UnityWebRequest.Post(Env.Api("user"), form))
        {
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
                    Session.session.SignIn(id.text, name.text, gender, iYear);
                    Session.session.isGeust=false;
                    SceneManager.LoadScene(2);
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                    msg.color = new Color(1, 0, 0);
                    msg.text = "Sign up failed. (Response processing error)";
                    isSignUp = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "Sign up failed. (Server connection error)";
                isSignUp = true;
            }
        }
    }
}
