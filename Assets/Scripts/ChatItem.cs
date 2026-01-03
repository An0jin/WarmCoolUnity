using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    Text msg;
    string _email;
    void Awake()
    {
        msg = GetComponent<Text>();
    }
    public string text
    {
        set => msg.text = value;
    }
    /// <summary>
    /// Sets the ID and updates the text color based on whether the session's user ID matches the provided ID.
    /// </summary>
    public string email
    {
        set
        {
            _email = value;
            msg.color = new Color(Session.session.Email == _email ? 1 : 0, 0, 0);
        }
    }
}
