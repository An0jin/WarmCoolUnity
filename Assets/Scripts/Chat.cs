using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Chat;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chat : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    Button submit;
    InputField input;
    bool isConn;
    void Awake()
    {
        isConn = false;
        chatClient = new ChatClient(this);
        submit = GameObject.Find("submit").GetComponent<Button>();
        input = GameObject.Find("input").GetComponent<InputField>();
        submit.onClick.AddListener(() =>
        {
            if (isConn)
                StartCoroutine(Submit());
                // chatClient.PublishMessage("WarmTone", input.text);
        });
        StartCoroutine(GetChat());
        // chatClient.Connect(Env.chatid, "1.0", new AuthenticationValues("test"));
        Application.runInBackground = true;

    }
    IEnumerator GetChat()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Env.Api($"chat/{Session.session.ColorId}")))
        // using (UnityWebRequest www = UnityWebRequest.Get(Env.Api($"chat/WarmTone")))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                JsonList<Message> list = JsonUtility.FromJson<JsonList<Message>>(www.downloadHandler.text);
                foreach (Message item in list.result)
                    AddMSG(item.name, item.msg);
                chatClient.Connect(Env.chatid, "1.0", new AuthenticationValues(Session.session.Name));
                isConn = true;
            }
        }
    }
    IEnumerator Submit()
    {
        chatClient.PublishMessage(Session.session.ColorId, input.text);
        WWWForm form = new WWWForm();
        form.AddField("user_id", Session.session.UserId);
        form.AddField("msg", input.text);
        input.text = "";
        using (UnityWebRequest www = UnityWebRequest.Post(Env.Api("chat"), form))
        {
            yield return www.SendWebRequest();
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(3);
        }
    
        chatClient.Service();
    }
    void AddMSG(string sender, object message)
    {
        Msg msg = Instantiate(Resources.Load<Msg>("msg"), transform);
        msg.text = $"{sender} : {message}";
        print($"{sender} : {message}");
    }
    public void OnConnected()
    {
        chatClient.Subscribe(Session.session.ColorId);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            AddMSG(senders[i], messages[i]);
        }
    }

    public void OnDisconnected()
    {
        chatClient.Disconnect();
    }
    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnChatStateChange(ChatState state)
    {

    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {

    }

    public void OnUnsubscribed(string[] channels)
    {

    }

    public void OnUserSubscribed(string channel, string user)
    {

    }

    public void OnUserUnsubscribed(string channel, string user)
    {

    }
}
