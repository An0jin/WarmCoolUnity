using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Toneiverse;

public class MakeUp : MonoBehaviour
{
    private ARFaceManager faceManager;
    [SerializeField, Range(0f, 255f)] private float alpha;

    void Awake()
    {
        faceManager = GetComponent<ARFaceManager>();

    }

    void OnEnable()
    {
        faceManager.trackablesChanged.AddListener(OnFaceChanged);
        Session.OnColorChanged += ApplyColorToAllFaces;
    }

    void OnDisable()
    {
        faceManager.trackablesChanged.RemoveListener(OnFaceChanged);
        Session.OnColorChanged -= ApplyColorToAllFaces;
    }

    private void ApplyColorToAllFaces()
    {
        foreach (ARFace face in faceManager.trackables)
        {
            face.GetComponent<MeshRenderer>().material.color = UpdateSingleFaceColor();
        }
    }

    // 2. 개별 얼굴에 색상 적용
    private Color UpdateSingleFaceColor()
    {
        Color color;
        ColorUtility.TryParseHtmlString(Session.session.HexCode, out color);
        color.a = alpha / 255f;
        return color;
    }

    void OnFaceChanged(ARTrackablesChangedEventArgs<ARFace> eventArgs)
    {
        // 새 얼굴이 추가될 때만 적용
        foreach (ARFace face in eventArgs.added)
        {
            face.GetComponent<MeshRenderer>().material.color = UpdateSingleFaceColor();
        }
    }
}