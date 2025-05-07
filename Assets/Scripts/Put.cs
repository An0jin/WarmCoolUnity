using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Put : MonoBehaviour
{
    Button put;
    InputField name, year, pw;
    bool isUpdate;
    Toggle man;
    Text msg,title;
    // Start is called before the first frame update
    void Start()
    {
        isUpdate = true;
        title = GameObject.Find("title").GetComponent<Text>();
        name = GameObject.Find("name").GetComponent<InputField>();
        year = GameObject.Find("year").GetComponent<InputField>();
        pw = GameObject.Find("pw").GetComponent<InputField>();
        man = GameObject.Find("man").GetComponent<Toggle>();
        put = GameObject.Find("Put").GetComponent<Button>();
        msg = GameObject.Find("msg").GetComponent<Text>();
        
        title.text=$"{Session.session.UserId}님의 정보";
        SetInputField(ref name,Session.session.Name);
        SetInputField(ref year,Session.session.Year);
        if(Session.session.Gender=="남자")
            man.isOn=true;
        
        put.onClick.AddListener(() =>
        {
            if (isUpdate)
                StartCoroutine(Update());
        });
    }
    void SetInputField(ref InputField inputField,object title){
        inputField.text=$"{title}";
    }
    IEnumerator Update()
    {
        string gender = man.isOn ? "남자" : "여자";
        int iYear=int.Parse(year.text);
        msg.color = new Color(1, 1, 1);
        msg.text = "수정중";
        isUpdate = false;
        if ( pw.text == "" || year.text == "" || name.text == "")
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "정보들을 입력해주세요";
            isUpdate = true;
            yield break;//끝내기
        }
        int today = DateTime.Today.Year;
        if (int.Parse(year.text) > today || int.Parse(year.text) < today - 100)
        {
            msg.color = new Color(1, 0, 0);
            msg.text = "태어난 연도가 이상합니다";
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
                    msg.color=new Color(1,1,1,json.result=="수정 완료"?1:0);
                    msg.text=json.result;
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                    msg.color = new Color(1, 0, 0);
                    msg.text = "회원가입에 실패했습니다. (응답 처리 오류)";
                    isUpdate = true;
                }
            }
            else
            {
                Debug.LogError("웹 요청 오류: " + www.error);
                msg.color = new Color(1, 0, 0);
                msg.text = "회원가입에 실패했습니다. (서버 연결 오류)";
                isUpdate = true;
            }
        }
    }
}
[Serializable]
public class UserInfo{
    public string user_id,pw,name,gender;
    public int year;
}
