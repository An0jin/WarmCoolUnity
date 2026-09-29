using UnityEngine;
using UnityEngine.Events;

/// <summary>버튼 클릭 시 인스펙터에 등록된 UnityEvent를 실행하는 범용 버튼입니다.</summary>
public class EventBtn : Btn
{
    [SerializeField] public UnityEvent onTriggered = new UnityEvent();

    protected override void OnClick()
    {
        onTriggered?.Invoke();
    }
}
