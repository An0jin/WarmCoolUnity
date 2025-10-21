using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class SceneBtn : MonoBehaviour
{
    [SerializeField]Scene scene;
    Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn=GetComponent<Button>();
        btn.onClick.AddListener(()=>{
            SceneManager.LoadScene((int)scene);
        });
    }
}
