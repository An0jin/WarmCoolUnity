using System.Collections;
using System.Collections.Generic;
using Toneiverse;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class SceneBtn : MonoBehaviour
{
    [SerializeField] protected SceneIndex scene;
    protected Button btn;
    // Start is called before the first frame update
    protected void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Click);
    }
    protected virtual void Click()
    {

        SceneManager.LoadScene((int)scene);
    }
}
