using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    InputField id,pw;
    Button SignIn,SignUp;
    bool isSignUp;
    Text msg;
    // Start is called before the first frame update
    void Start()
    {
        isSignUp=true;
        id=GameObject.Find("id").GetComponent<InputField>();
        pw=GameObject.Find("pw").GetComponent<InputField>();
        SignIn=GameObject.Find("SignIn").GetComponent<Button>();
        SignUp=GameObject.Find("SignUp").GetComponent<Button>();
        msg=GameObject.Find("msg").GetComponent<Text>();
        SignIn.onClick.AddListener(()=>{
            if(isSignUp)
                StartCoroutine(LogIn());
        });
        SignUp.onClick.AddListener(()=>{
            SceneManager.LoadScene(1);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LogIn(){
        isSignUp=false;
        if(id.text==""||pw.text==""){
            msg.text="아이디와 비밀번호를 입력해주세요";
            isSignUp=true;
            yield return null;
        }
        WWWForm form=new WWWForm();
        form.AddField("user_id",id.text);
        form.AddField("pw",pw.text);
        using(UnityWebRequest www=UnityWebRequest.Post(Env.Api("login"),form)){
            yield return www.SendWebRequest();
            if(www.result==UnityWebRequest.Result.Success){
                InfoJson json=JsonUtility.FromJson<InfoJson>(www.downloadHandler.text);
                if(json.msg=="성공"){
                    Session.session.Login(json);
                    
                }else{
                    msg.text=json.msg;
                }
            }
        }
    }
}
