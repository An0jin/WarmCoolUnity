using Photon.Pun;
using UnityEngine;

public class Session : MonoBehaviour
{
    [SerializeField]ServerSettings server;
    private static Session _instance;
    public static Session session
    {
        get
        {
            if (_instance == null)
            {
                // 씬에 이미 존재하는 Session 인스턴스 찾기
                _instance = FindObjectOfType<Session>();

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

    
    public string UserId { get; private set; }
    public string Name { get; private set; }
    public string Gender { get; private set; }
    public int Year { get; private set; }
    public string ColorId { get; private set; }
    public string HexCode { get;  set; }
    public string Description { get; private set; }
    public bool isGeust{get;set;}
    public string chatid { 
        get=>server.AppSettings.AppIdChat;
        set=>server.AppSettings.AppIdChat=value;
    }
    public string punid { 
        get=>server.AppSettings.AppIdRealtime;
        set=>server.AppSettings.AppIdRealtime=value;
    }


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
        UserId = json.user_id;
        Name = json.name;
        Gender = json.gender;
        Year = json.year;
        ColorId = json.color_id;
        HexCode = json.hex_code;
        Description = json.description;
    }

    public void SignIn(string userId, string name, string gender, int year)
    {
        UserId = userId;
        Name = name;
        Gender = gender;
        Year = year;
    }

    public void UpdateInfo(string name, string gender, int year)
    {
        Name = name;
        Gender = gender;
        Year = year;
    }

    public void Predict(ColorJson json)
    {
        HexCode = json.hex_code;
        ColorId = json.color_id;
        Description = json.description;
    }

}
