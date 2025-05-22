using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Msg : MonoBehaviour
{
    Text msg;
    string _id;
    void Awake()
    {
        msg=GetComponent<Text>();
    }
    public string text{
        set=>msg.text=value;
    }
    /// <summary>
/// Sets the ID and updates the text color based on whether the session's user ID matches the provided ID.
/// </summary>
    public string id{
        set{
            _id=value;
            msg.color=new Color(Session.session.UserId==_id?1:0,0,0);
        }
    }
}
