using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Toneiverse.DTO;

public class ColorView : MonoBehaviour
{
    [SerializeField] ResultText cnameText;
    void Start()
    {
        NavigationManager.navigationManager.ClearHistory();
        StartCoroutine(APIManager.Get($"/lipstick/{Session.session.ColorId}", (jsonText) =>
        {
            JsonList<ColorJson> json = JsonUtility.FromJson<JsonList<ColorJson>>(jsonText);
            foreach (var item in json.result)
            {
                ColorBtn btn = Instantiate(Resources.Load<ColorBtn>("ColorBtn"), transform);
                btn.SetBtnColor(item.hex_code, item.cname, cnameText);
            }
        }));
    }
}
