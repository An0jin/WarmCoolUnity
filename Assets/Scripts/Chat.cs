using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Chat;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    Text msg;
    InputField input;
    void Awake()
    {
        msg = GetComponent<Text>();
        input = GameObject.Find("input").GetComponent<InputField>();
        chatClient = new ChatClient(this);
        Session.session.chatid = Env.chatid;
        Session.session.punid = Env.punid;
        chatClient.Connect(Session.session.chatid, "1.0", new AuthenticationValues("빌드"));

    }
    void Update()
    {
        if (chatClient != null)
            chatClient.Service();
        if (Input.GetKey(KeyCode.Return) && chatClient.CanChat && input.text != "")
        {
            chatClient.PublishMessage("master", input.text);
            input.text = "";
        }
    }

    public void OnConnected()
    {
        chatClient.Subscribe("master");
    }

    public void OnDisconnected()
    {
        chatClient.Disconnect();
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            string row = $"{senders[i]}:{messages[i]}\n";
            print(row);
            msg.text += row;

        }
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
