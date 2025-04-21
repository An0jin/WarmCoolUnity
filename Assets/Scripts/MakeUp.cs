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
    public void SetColor(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);
        foreach (ARFace face in faceManager.trackables)
        {

            var meshRenderer = face.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material.color = color;
            }
        }
    }
}
