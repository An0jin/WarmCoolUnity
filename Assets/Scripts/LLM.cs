using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LLM : MonoBehaviour
{
    Button cls;
    Button open;
    Button submit;
    InputField prompt;
    bool canSubmit;

    void Awake()
    {
        canSubmit = true;
        cls = transform.GetChild(2).GetComponent<Button>();
        open = GameObject.Find("open").GetComponent<Button>();
        prompt = transform.GetChild(0).GetComponent<InputField>();
        submit = transform.GetChild(1).GetComponent<Button>();
        gameObject.SetActive(false);
        cls.onClick.AddListener(() =>
        {
            if (canSubmit)
                gameObject.SetActive(false);
        });
        open.onClick.AddListener(() =>
        {
            gameObject.SetActive(true);
        });
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
                gameObject.SetActive(false);
            }else{
                canSubmit = true;
                gameObject.SetActive(false);
                print(www.error);
            }
        }
    }
}
