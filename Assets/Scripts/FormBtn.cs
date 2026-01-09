using System;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public abstract class FormBtn : MSGBtn
{
    [SerializeField] protected InputField pw, pwConfirm, name, year;
    [SerializeField] protected Toggle man;
    protected string sex => man.isOn ? "남자" : "여자";
    protected virtual bool MatchPw(string pw)
    {
        string pwPattern = "^[a-zA-Z0-9`~!@#$%^&*()_\\-+=\\[\\]{}|;:'\",<.>/?]{8,16}$";
        if (string.IsNullOrEmpty(pw))
            return false;
        return Regex.IsMatch(pw, pwPattern);
    }
    protected virtual bool CheckPw => pw.text == pwConfirm.text;
    protected virtual bool IsNull()
    {
        return string.IsNullOrEmpty(pw.text) || string.IsNullOrEmpty(pwConfirm.text) || string.IsNullOrEmpty(name.text);
    }
    protected virtual bool ValidateForm()
    {
        if (IsNull())
        {
            Error("모든 정보를 입력해주세요.");
            return false;
        }
        int todayTear = DateTime.Now.Year;
        int birth = int.Parse(year.text);
        if (1 > todayTear - birth || todayTear - birth > 120)
        {
            Error("태어난 연도가 이상합니다");
            return false;
        }
        if (!MatchPw(pw.text))
        {
            Error("비밀번호는 영문과 숫자, 특수문자로 구성되어야 하며 8~16자리여야 합니다.");
            return false;
        }
        if (!CheckPw)
        {
            Error("비밀번호가 일치하지 않습니다.");
            return false;
        }
        return true;
    }
}
