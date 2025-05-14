using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        string gender = man.isOn ? "남자" : "여자";
        int iYear=int.Parse(year.text);
        msg.color = new Color(1, 1, 1);
        msg.text = "회원가입 하는중";
        isSignUp = false;
        if (id.text == "" || pw.text == "" || year.text == "" || name.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "정보들을 입력해주세요";
            isSignUp = true;
            yield break;//끝내기
        }
        int today = DateTime.Today.Year;
        if (int.Parse(year.text) > today || int.Parse(year.text) < today - 100)
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "태어난 연도가 이상합니다";
            isSignUp = true;
            yield break;//끝내기
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
                    msg.text = "회원가입에 실패했습니다. (응답 처리 오류)";
                    isSignUp = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "회원가입에 실패했습니다. (서버 연결 오류)";
                isSignUp = true;
            }
        }
    }
}
