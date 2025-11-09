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
    public string Email { get; private set; }
    public string ColorId { get; private set; }
    public string HexCode {set;get;}
    public string Description { get; private set; }
    public bool isGeust{get;set;}
    public string PhotonChatId { 
        set=>server.AppSettings.AppIdChat=value;
    }
    public string PhotonAppId { 
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
        Name = json.name;
        Email = json.email;
        ColorId = json.color_id;
        HexCode = json.hex_code;
        Description = json.description;
        Token = json.token;
    }
    public void LogOut()
    {
        Name = "";
        Email = "";
        ColorId = "";
        HexCode = "";
        Description = "";
        Token = "";
    }


    public void SignIn(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public void UpdateInfo(string name)
    {
        Name = name;
    }

    public void Predict(ColorJson json)
    {
        HexCode = json.hex_code;
        ColorId = json.color_id;
        Description = json.description;
    }

}
