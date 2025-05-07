using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text=GetComponent<Text>();
        text.text=$" 당신의 피부톤 : {Session.session.ColorId}";
    }
}
