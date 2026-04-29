using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Env", menuName = "Env/Setting")]
public class EnvConfig : ScriptableObject
{
    [Header("API Settings")]
    [SerializeField] private string api;
    public string Api(string endpoint)
    {
        return api + (endpoint.StartsWith("/") ? endpoint.Substring(1) : endpoint);
    }
    [Header("Photon Settings")]
    [SerializeField] private string photonChatid;
    [SerializeField] private string photonAppid;
    [Header("user data file")]
    [SerializeField] private string fname;
    public string PhotonChatId => photonChatid;
    public string PhotonAppId => photonAppid;
    public string FilePath => Path.Combine(Application.persistentDataPath, fname);
}
