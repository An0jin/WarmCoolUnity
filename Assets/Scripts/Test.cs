using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Button btn;
    bool canClick=true;
    // Start is called before the first frame update
    void Start()
    {
        btn=GetComponent<Button>();
        btn.onClick.AddListener(()=>{
            if(canClick)
                StartCoroutine(Capture());
        });
    }
    IEnumerator Capture(){
        canClick=false;
        Text text=transform.GetChild(0).GetComponent<Text>();
        text.text="";
        Image btn_img=GetComponent<Image>();
        btn_img.color=new Color(1,1,1,0);
        yield return new WaitForEndOfFrame();
        btn_img.color=new Color(1,1,1,1);
        text.text="로딩중";
        
    }
    IEnumerator Submit(){
        canClick=false;
        Text text=transform.GetChild(0).GetComponent<Text>();
        text.text="";
        Image btn_img=GetComponent<Image>();
        btn_img.color=new Color(1,1,1,0);
        yield return new WaitForEndOfFrame();
        btn_img.color=new Color(1,1,1,1);
        text.text="로딩중";
        
    }
}
