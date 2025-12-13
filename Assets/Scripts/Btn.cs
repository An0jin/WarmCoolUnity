using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public abstract class Btn : MonoBehaviour
{
    protected Button btn;

    // 2. Awake 권장: Start보다 먼저 실행되어 초기화 순서에서 안전함
    protected virtual void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    // 3. abstract method: "자식들아, 내용은 너희가 무조건 채워라. 안 채우면 에러 낼 거야."
    protected abstract void OnClick();
}