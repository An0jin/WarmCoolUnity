using UnityEngine;
using UnityEngine.UI;

public class CnameText : MonoBehaviour
{
    Text _txt;
    void Start()
    {
        _txt = GetComponent<Text>();
        _txt.text = $"당신의 립스틱 색상 : {Session.session.Cname}";
    }
    //버튼 클릭시 정보 변경
    public string txt
    {

        set
        {
            Session.session.Cname = value;
            _txt.text = $"당신의 립스틱 색상 : {value}";
        }

    }
}
