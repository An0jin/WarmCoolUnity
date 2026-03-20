using UnityEngine;
using Toneiverse.DTO;
using System;

public class Session : MonoBehaviour
{
    private static Session _instance;
    public static Session session
    {
        get
        {
            if (_instance == null)
            {
                // Find existing Session instance in the scene
                _instance = FindFirstObjectByType<Session>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Session).Name);
                    _instance = singletonObject.AddComponent<Session>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }


    public string Token { get; private set; }
    public string Name { get; private set; }
    public string Sex { get; private set; }
    public string Year { get; private set; }
    public string Email { get; private set; }
    public string ColorId { get; private set; }
    public string Cname { get; set; }
    private string _hexCode;
    public string HexCode
    {
        set
        {
            if (_hexCode != value)
            {
                _hexCode = value;
                OnColorChanged?.Invoke();
            }
        }
        get => _hexCode;
    }
    public static Action OnColorChanged;
    private void Awake()
    {
        // Application.runInBackground = true;
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Login(InfoJson json)
    {
        Name = json.name;
        Email = json.email;
        ColorId = json.color_id;
        HexCode = json.hex_code;
        Token = json.token;
        Cname = json.cname;
        Sex = json.sex;
        Year = json.year;
    }
    public void LogOut()
    {
        NavigationManager.navigationManager.ClearHistory();
        Name = "";
        Email = "";
        ColorId = "";
        HexCode = "";
        Token = "";
        Cname = "";
        Sex = "";
        Year = "";
    }


    public void SignIn(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public void UpdateInfo(string name, string sex, string year, string token)
    {
        Name = name;
        Sex = sex;
        Year = year;
        Token = token;
    }
    //어차피 지울 함수
    public void AAA(string sex, string year)
    {
        Sex = sex;
        Year = year;
    }

    public void Predict(ColorJson json)
    {
        HexCode = json.hex_code;
        ColorId = json.color_id;
        Cname = json.cname;
    }

}
