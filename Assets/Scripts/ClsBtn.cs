using UnityEngine;
using UnityEngine.UI;

public class ClsBtn : MonoBehaviour
{
    [SerializeField] GameObject obj;
    Button btn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn = GetComponent<Button>();
        obj.SetActive(false);
        btn.onClick.AddListener(() => obj.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
