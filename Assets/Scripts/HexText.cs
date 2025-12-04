using UnityEngine;
using UnityEngine.UI;

public class HexText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        Text txt = GetComponent<Text>();
        txt.text = $"당신의 립스틱 색상 : {Session.session.Cname}";          
    }
}
