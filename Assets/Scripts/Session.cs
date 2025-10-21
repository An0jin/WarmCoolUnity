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

    
    public string Token { get; private set; }
    public string UserId { get; private set; }
    public string Name { get; private set; }
    public string Gender { get; private set; }
    public string Email { get; private set; }
    public string ColorId { get; private set; }
    public string HexCode {set;get;}
    public string Description { get; private set; }
    public bool isGeust{get;set;}
    public string chatid { 
        set=>server.AppSettings.AppIdChat=value;
    }
    public string punid { 
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
        Email = json.email;
        ColorId = json.color_id;
        HexCode = json.hex_code;
        Description = json.description;
        Token = json.token;
    }

    public void SignIn(string userId, string name, string gender, string email)
    {
        UserId = userId;
        Name = name;
        Gender = gender;
        Email = email;
    }

    public void UpdateInfo(string name, string gender, string email)
    {
        Name = name;
        Gender = gender;
        Email = email;
    }

    public void Predict(ColorJson json)
    {
        HexCode = json.hex_code;
        ColorId = json.color_id;
        Description = json.description;
    }

}
