using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Form : MonoBehaviour
{
    Button put,delete;
    InputField name, year, pw;
    bool isUpdate,isDelete;
    Toggle man;
    Text msg;
    // Start is called before the first frame update
    void Start()
    {
        isUpdate = true;
        isDelete=true;
        name = GameObject.Find("name").GetComponent<InputField>();
        year = GameObject.Find("year").GetComponent<InputField>();
        pw = GameObject.Find("pw").GetComponent<InputField>();
        man = GameObject.Find("man").GetComponent<Toggle>();
        put = GameObject.Find("Put").GetComponent<Button>();
        delete = GameObject.Find("del").GetComponent<Button>();
        msg = GameObject.Find("msg").GetComponent<Text>();
        
        SetInputField(ref name,Session.session.Name);
        SetInputField(ref year,Session.session.Year);
        if(Session.session.Gender=="Male")
            man.isOn=true;
        
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
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(3);
        }
    }
    void SetInputField(ref InputField inputField,object title){
        inputField.text=$"{title}";
    }
    IEnumerator Put()
    {
        string gender = man.isOn ? "Male" : "Female";
        int iYear=int.Parse(year.text);
        msg.color = new Color(1, 1, 1);
        msg.text = "Updating...";
        isUpdate = false;
        if ( pw.text == "" || year.text == "" || name.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "Please enter all information";
            isUpdate = true;
            yield break;//끝내기
        }
        int today = DateTime.Today.Year;
        if (int.Parse(year.text) > today || int.Parse(year.text) < today - 100)
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "Birth year is not valid";
            isUpdate = true;
            yield break;//끝내기
        }
        UserInfo user=new UserInfo();
        user.gender=gender;
        user.name=name.text;
        user.pw=pw.text;
        user.user_id=Session.session.UserId;
        user.year=iYear;
        using (UnityWebRequest www = UnityWebRequest.Put(Env.Api("user"), JsonUtility.ToJson(user)))
        {
            www.SetRequestHeader("Content-Type","application/json");
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
                    Session.session.UpdateInfo(name.text, gender, iYear);
                    msg.color=new Color(1,1,1,json.result=="Update complete"?1:0);
                    msg.text=json.result;
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                    msg.color = new Color(1, 0, 0);
                    msg.text = "Sign up failed. (Response processing error)";
                    isUpdate = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "Sign up failed. (Server connection error)";
                isUpdate = true;
            }
        }
    }
    IEnumerator Delete()
    {
        using (UnityWebRequest www = UnityWebRequest.Delete(Env.Api($"user/{Session.session.UserId}")))
        {
            www.downloadHandler=new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            // 디버그 로그 추가
            Debug.Log("서버 응답 코드: " + www.responseCode);
            Debug.Log("서버 응답 내용: " + www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {   
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "Server connection failed";
                isDelete = true;
            }
        }
    }
}
[Serializable]
public class UserInfo{
    public string user_id,pw,name,gender;
    public int year;
}
