using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LLM : MonoBehaviour
{
    Button submit;
    InputField prompt;
    Button cls;
    bool canSubmit;

    void Awake()
    {
        canSubmit = true;
        prompt = transform.GetChild(0).GetComponent<InputField>();
        submit = transform.GetChild(1).GetComponent<Button>();
        cls = transform.GetChild(2).GetComponent<Button>();

        submit.onClick.AddListener(() =>
        {
            if (canSubmit)
                StartCoroutine(Submit());
        });
    }
    IEnumerator Submit()
    {
        WWWForm form = new WWWForm();
        canSubmit = false;
        form.AddField("user_id", Session.session.UserId);
        form.AddField("msg", prompt.text);
        form.AddField("color_id", Session.session.ColorId);
        prompt.text="";
        Text placeholder=prompt.placeholder.GetComponent<Text>();
        string placeholder_text=placeholder.text;
        placeholder.text="AI가 생각하고 있습니다.";
        prompt.interactable=false;
        submit.interactable=false;
        cls.interactable=false;
        using (UnityWebRequest www = UnityWebRequest.Post(Env.Api("llm"), form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                print(json);
                Json<string> colorJson = JsonUtility.FromJson<Json<string>>(json);
                Session.session.HexCode = colorJson.result;
                canSubmit = true;
                placeholder.text=placeholder_text;
                prompt.interactable=true;
                submit.interactable=true;
                cls.interactable=true;
                gameObject.SetActive(false);
            }else{
                canSubmit = true;
                placeholder.text=placeholder_text;
                prompt.interactable=true;
                submit.interactable=true;
                cls.interactable=true;
                print(www.error);
            }
        }
    }
}
