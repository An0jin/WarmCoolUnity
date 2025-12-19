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
using Toneiverse.DTO;

public class Chat : Btn, IChatClientListener
{
    private ChatClient chatClient;
    [SerializeField] InputField input;
    bool isConn;
    void Awake()
    {
        base.Awake();
        isConn = false;
        chatClient = new ChatClient(this);
        GetChat();
        Application.runInBackground = true;

    }
    protected override void OnClick()
    {
        if (isConn)
        {

            chatClient.PublishMessage(Session.session.ColorId, input.text);
            WWWForm form = new WWWForm();
            form.AddField("token", Session.session.Token);
            form.AddField("msg", input.text);
            form.AddField("color_id", Session.session.ColorId);
            input.text = "";
            StartCoroutine(APIManager.Post("chat", form));
        }
    }

    void GetChat()
    {
        StartCoroutine(APIManager.Get($"chat/{Session.session.ColorId}", (jsonText) =>
        {
            JsonList<Message> list = JsonUtility.FromJson<JsonList<Message>>(jsonText);
            foreach (Message item in list.result)
                AddMSG(item.name, item.msg);
            chatClient.Connect(Env.photonChatid, "1.0", new AuthenticationValues(Session.session.Name));
            isConn = true;
        }));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
