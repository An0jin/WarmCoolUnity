using System.Collections.Generic; // Stack 자료구조 사용을 위한 System.Collections.Generic 참조
using Toneiverse; // SceneIndex 열거형이 선언된 프로젝트 네임스페이스 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.SceneManagement; // Unity 씬전환 네임스페이스 참조

/// <summary>방문한 씬을 스택으로 기록해 앞·뒤 화면 이동을 관리합니다.</summary>
public class NavigationManager : MonoBehaviour // 씬 이동 이력을 LIFO 스택으로 관리하는 싱글턴
{
    private Stack<SceneIndex> _sceneStack = new Stack<SceneIndex>(); // 이전 씬 이력 저장용 스택
    private static NavigationManager sceneStack; // 싱글턴 정적 객체 참조

    // 전역 접근용 싱글턴 프로퍼티
    public static NavigationManager navigationManager
    {
        get
        {
            if (sceneStack == null)
            {
                // 씬에서 NavigationManager 검색
                sceneStack = FindFirstObjectByType<NavigationManager>();
                if (sceneStack == null)
                {
                    // 씬에 없으면 런타임에 새로 생성
                    GameObject obj = new GameObject("NavigationManager");
                    sceneStack = obj.AddComponent<NavigationManager>();
                }
            }
            return sceneStack;
        }
    }

    // 이전 방문 기록(스택) 전체 삭제
    public void ClearHistory()
    {
        navigationManager._sceneStack.Clear();
    }

    // 싱글턴 초기화 및 DontDestroyOnLoad 적용
    private void Awake()
    {
        if (sceneStack != null && sceneStack != this)
        {
            Destroy(gameObject); // 중복 오브젝트 파괴
            return;
        }
        sceneStack = this;
        DontDestroyOnLoad(gameObject); // 씬 변경 시에도 유지
    }

    // 다음 씬으로 전진 이동 (현재 씬을 이력 스택에 저장)
    public void Front(SceneIndex scene)
    {
        SceneIndex index = (SceneIndex)SceneManager.GetActiveScene().buildIndex; // 현재 활성 씬 인덱스
        if (_sceneStack.Count == 0 || _sceneStack.Peek() != index)
        {
            _sceneStack.Push(index); // 이전 씬 이력 푸시
        }
        SceneManager.LoadScene((int)scene); // 목적지 씬 전환
    }

    // 이전 씬으로 후퇴 이동 (스택에서 팝)
    public void Back()
    {
        if (_sceneStack.Count > 0)
        {
            SceneIndex index = _sceneStack.Pop(); // 최근 방문 씬 팝
            SceneManager.LoadScene((int)index); // 해당 이전 씬으로 복귀
        }
        else
            Application.Quit(); // 돌아갈 이전 씬이 없다면 앱 종료
    }
}
