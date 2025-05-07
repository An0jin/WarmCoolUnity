using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MakeUp : MonoBehaviour
{
    ARFaceManager faceManager;
    // Start is called before the first frame update
    void Start()
    {
        faceManager = GetComponent<ARFaceManager>();
    }
    
    void Update()
    {
        Color color;
        ColorUtility.TryParseHtmlString(Session.session.HexCode,out color);
        foreach (ARFace face in faceManager.trackables)
        {

            var meshRenderer = face.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.material.color = color;
        }
        
    }
}
