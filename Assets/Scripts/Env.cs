using UnityEngine;
public class Env : MonoBehaviour
{
    public static Env I { get; private set; }

    [SerializeField] private EnvConfig config;

    public EnvConfig Config => config;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;

        // 씬이 바뀌어도 파괴되지 않도록 설정 (선택 사항)
        DontDestroyOnLoad(gameObject);
    }

}
