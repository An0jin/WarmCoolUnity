using UnityEngine;
using UnityEngine.UI;

public class Privacy : MonoBehaviour
{
    Button btn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Application.OpenURL("https://an0jin.github.io/Toniverse/privacy.html");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
