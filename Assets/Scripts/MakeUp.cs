using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.XR.ARFoundation; // ARFoundation 얼굴 인식 패키지 참조
using Toneiverse; // 세션 데이터 참조

/// <summary>추적 중인 AR 얼굴 재질에 선택한 립스틱 색상을 적용합니다.</summary>
public class MakeUp : MonoBehaviour // AR 카메라 기반 입술 재질 립스틱 착색 스크립트
{
    private ARFaceManager faceManager; // ARFaceManager 컴포넌트 필드
    [SerializeField, Range(0f, 255f)] private float alpha; // 립스틱 투명도 값 (0 ~ 255)

    // 컴포넌트 획득 초기화
    void Awake()
    {
        faceManager = GetComponent<ARFaceManager>();
    }

    // 이벤트 리스너 등록
    void OnEnable()
    {
        faceManager.trackablesChanged.AddListener(OnFaceChanged);
        Session.OnColorChanged += ApplyColorToAllFaces; // 립스틱 색상 변경 시 자동 갱신
    }

    // 이벤트 리스너 해제
    void OnDisable()
    {
        faceManager.trackablesChanged.RemoveListener(OnFaceChanged);
        Session.OnColorChanged -= ApplyColorToAllFaces;
    }

    // 감지된 모든 얼굴 매시에 색상 적용
    private void ApplyColorToAllFaces()
    {
        foreach (ARFace face in faceManager.trackables)
        {
            face.GetComponent<MeshRenderer>().material.color = UpdateSingleFaceColor();
        }
    }

    // HEX 및 Alpha 통합 Color 객체 반환
    private Color UpdateSingleFaceColor()
    {
        Color color;
        ColorUtility.TryParseHtmlString(Session.session.HexCode, out color);
        color.a = alpha / 255f; // Alpha 값 비율 변환
        return color;
    }

    // 신규 얼굴 포착 시 색상 지정
    void OnFaceChanged(ARTrackablesChangedEventArgs<ARFace> eventArgs)
    {
        foreach (ARFace face in eventArgs.added)
        {
            face.GetComponent<MeshRenderer>().material.color = UpdateSingleFaceColor();
        }
    }
}
