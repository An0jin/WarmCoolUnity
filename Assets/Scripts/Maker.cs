using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Maker : MonoBehaviour
{
    GameObject maker;
    Button btn;
    [SerializeField] bool isShow;
    // Start is called before the first frame update
    void Awake()
    {
        btn=GetComponent<Button>();
        maker=GameObject.Find("maker");
        if(isShow)
            maker.SetActive(false);
        btn.onClick.AddListener(()=>{
            maker.SetActive(isShow);
        });
    }
}
