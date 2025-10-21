using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetPW : MonoBehaviour
{
    Button btn;
    [SerializeField] InputField email;
    [SerializeField] Text msg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn=GetComponent<Button>();
        btn.onClick.AddListener(()=>{
            StartCoroutine(Submit());
        });
    }
    IEnumerator Submit()
    {
        WWWForm form=new WWWForm();
        form.AddField("email", email.text);
        msg.text="아이디와 비밀번호 찾는중...";
        using(UnityWebRequest www=UnityWebRequest.Post(Env.Api("email"),form))
        {
            yield return www.SendWebRequest();
            if(www.result==UnityWebRequest.Result.Success)
            {
                Json<string> json=JsonUtility.FromJson<Json<string>>(www.downloadHandler.text);
                msg.text=json.result;
            }
            else
            {
                msg.text = "로그인 실패. (서버 연결 오류)";
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
