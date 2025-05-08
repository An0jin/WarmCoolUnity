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
    bool 보내기;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        chatClient.Connect(Env.chatid,"1.0",new AuthenticationValues(Session.session.Name));
        chatClient.Service();

    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(chatClient.CanChat&&보내기){
            chatClient.PublishMessage(Session.session.ColorId,"안녕하세요");
        }
    }
    public void OnConnected()
    {
        chatClient.Subscribe(Session.session.ColorId);
    }

    public void OnDisconnected()
    {
        chatClient.Disconnect();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        
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
