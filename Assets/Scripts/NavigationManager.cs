using System.Collections.Generic;
using Toneiverse;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    private Stack<SceneIndex> _sceneStack = new Stack<SceneIndex>();
    public static NavigationManager sceneStack;
    public static NavigationManager navigationManager
    {
        get
        {
            if (sceneStack == null)
            {
                // 최신 유니티 권장 메서드 사용
                sceneStack = FindFirstObjectByType<NavigationManager>();
                if (sceneStack == null)
                {
                    GameObject obj = new GameObject("NavigationManager");
                    sceneStack = obj.AddComponent<NavigationManager>();
                }
            }
            return sceneStack;
        }
    }
    public void ClearHistory()
    {
        navigationManager._sceneStack.Clear();
    }
    private void Awake()
    {
        if (sceneStack != null && sceneStack != this)
        {
            Destroy(gameObject);
            return;
        }
        sceneStack = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Front(SceneIndex scene)
    {
        SceneIndex index = (SceneIndex)SceneManager.GetActiveScene().buildIndex;
        if (_sceneStack.Count == 0 || _sceneStack.Peek() != index)
        {
            _sceneStack.Push(index);
        }
        SceneManager.LoadScene((int)scene);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Debugging();
    }
    public void Debugging()
    {
#if UNITY_EDITOR
        if (_sceneStack.Count == 0)
        {
            Debug.Log("[Nav] Stack is empty.");
            return;
        }

        // 2. 한 줄에 모든 경로를 시각적으로 표현 (LIFO 순서)
        string history = string.Join(" -> ", _sceneStack);
        Debug.Log($"[Nav] Current Stack (Top to Bottom): {history}");
#endif
    }
    public void Back()
    {
        if (_sceneStack.Count > 0)
        {
            SceneIndex index = _sceneStack.Pop();
            SceneManager.LoadScene((int)index);
        }
        else
        {
            print("나간다 ㅂㅇㅂㅇ");
            Application.Quit();
        }
    }
}
