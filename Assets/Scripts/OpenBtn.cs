using UnityEngine;
using UnityEngine.UI;

public class OpenBtn : MonoBehaviour
{
    [SerializeField] GameObject obj;
    Button btn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => obj.SetActive(true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
