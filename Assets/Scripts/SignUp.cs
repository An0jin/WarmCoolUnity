using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    Button back, signUP;
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
        back = GameObject.Find("back").GetComponent<Button>();
        man = GameObject.Find("man").GetComponent<Toggle>();
        signUP = GameObject.Find("SignUP").GetComponent<Button>();
        msg = GameObject.Find("msg").GetComponent<Text>();
        back.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
        signUP.onClick.AddListener(() =>
        {
            if (isSignUp)
                StartCoroutine(SignUP());
        });
    }
    IEnumerator SignUP()
    {
        string gender= man.isOn ? "남자" : "여자";
        msg.color=new Color(1,1,1);
        msg.text="회원가입 하는중";
        isSignUp = false;
        if (id.text == "" || pw.text == "" || year.text == "" || name.text == "")
        {
        msg.color=new Color(1,0,0);
            msg.text = "정보들을 입력해주세요";
            isSignUp = true;
            yield break;//끝내기
        }
        int today = DateTime.Today.Year;
        if (int.Parse(year.text) > today || int.Parse(year.text) < today - 100)
        {
        msg.color=new Color(1,0,0);
            msg.text = "태어난 연도가 이상합니다";
            isSignUp = true;
            yield break;//끝내기
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", id.text);
        form.AddField("pw", pw.text);
        form.AddField("name", name.text);
        form.AddField("year", year.text);
        form.AddField("gender",gender);
        using (UnityWebRequest www = UnityWebRequest.Post(Env.Api("user"), form))
        {
            yield return www.SendWebRequest();
            Json<string> json = JsonUtility.FromJson<Json<string>>(www.downloadHandler.text);
            if(www.result==UnityWebRequest.Result.Success){
            
            Session.session.SignIn(id.text,name.text,gender,int.Parse(year.text));
            SceneManager.LoadScene(2);
            }else{
            msg.color=new Color(1,0,0);
            msg.text = "회원가입에 실패하셨습니다.";
            isSignUp = true;
            }
        }
    }
}
