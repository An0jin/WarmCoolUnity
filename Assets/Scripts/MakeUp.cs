using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeUp : MonoBehaviour
{
    MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh=GetComponent<MeshRenderer>();
    }
    public void SetColor(string hex){
        Color color;
        ColorUtility.TryParseHtmlString(hex,out color);
        mesh.material.color=color;
    }
}
