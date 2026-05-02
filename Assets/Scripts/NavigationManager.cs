using System.Collections.Generic;
using Toneiverse;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    private Stack<SceneIndex> _sceneStack = new Stack<SceneIndex>();
    private static NavigationManager sceneStack;
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
    public void Back()
    {
        if (_sceneStack.Count > 0)
        {
            SceneIndex index = _sceneStack.Pop();
            SceneManager.LoadScene((int)index);
        }
        else
            Application.Quit();

    }
}
