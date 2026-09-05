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
using Photon.Pun;

public class Chat : Btn, IChatClientListener
{
    private ChatClient chatClient;
    [SerializeField] GameObject msgView;
    [SerializeField] InputField input;
    bool isConn;
    protected override void Awake()
    {
        base.Awake();
        isConn = false;
        if (!ChatManager.chatManager.IsAppIdConfigured)
        {
            ChatManager.chatManager.PhotonChatId = Env.I.Config.PhotonChatId;
            ChatManager.chatManager.PhotonAppId = Env.I.Config.PhotonAppId;
        }
        chatClient = new ChatClient(this);
        GetChat();
        Application.runInBackground = true;

    }
    protected override void OnClick()
    {
        if (isConn && input.text.Replace(" ", "") != "")
        {

            chatClient.PublishMessage(Session.session.ColorId, input.text);
            WWWForm form = new WWWForm();
            form.AddField("token", Session.session.Token);
            form.AddField("msg", input.text);
            form.AddField("color_id", Session.session.ColorId);
            input.text = "";
            StartCoroutine(APIManager.Post("chat", form, (s) =>
            {
                print("성공");
            }));
        }
    }

    void GetChat()
    {
        StartCoroutine(APIManager.Get($"chat/{Session.session.ColorId}", (jsonText) =>
        {
            JsonList<Message> list = JsonUtility.FromJson<JsonList<Message>>(jsonText);
            foreach (Message item in list.result)
                AddMSG(item.name, item.msg);
            chatClient.Connect(Env.I.Config.PhotonChatId, "1.0", new AuthenticationValues(Session.session.Name));
            isConn = true;
        }));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
            chatClient.Disconnect();
            SceneManager.LoadScene(3);
        }

        chatClient.Service();
    }
    void AddMSG(string sender, object message)
    {
        ChatItem msg = Instantiate(Resources.Load<ChatItem>("msg"), msgView.transform);
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
