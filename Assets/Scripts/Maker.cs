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
        /// <summary>
        /// Initializes the maker button.
        /// </summary>
        /// <remarks>
        /// This function is called when the script is loaded.
        /// It sets up the button, finds the maker game object, and
        /// sets up the button's OnClick event to toggle the maker's visibility.
        /// </remarks>
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
