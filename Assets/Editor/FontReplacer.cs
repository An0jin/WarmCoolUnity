using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class FontReplacer : EditorWindow
{
    private Font targetFont;

    [MenuItem("Tools/Toneiverse/Replace All Fonts Window")]
    public static void ShowWindow()
    {
        GetWindow<FontReplacer>("Font Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("일괄 폰트 교체 설정", EditorStyles.boldLabel);

        // 에디터 상에서 폰트를 직접 선택할 수 있는 필드
        targetFont = (Font)EditorGUILayout.ObjectField("대상 폰트", targetFont, typeof(Font), false);

        if (GUILayout.Button("모든 씬/프리팹 폰트 교체"))
        {
            if (targetFont == null)
            {
                EditorUtility.DisplayDialog("오류", "교체할 폰트를 먼저 선택하십시오.", "확인");
                return;
            }

            if (EditorUtility.DisplayDialog("경고", "모든 씬과 프리팹의 폰트가 교체됩니다. 계속하시겠습니까?", "예", "아니오"))
            {
                ExecuteReplace();
            }
        }
    }

    private void ExecuteReplace()
    {
        // 1. 모든 씬 처리
        string[] allSceneGuids = AssetDatabase.FindAssets("t:Scene");
        foreach (string guid in allSceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            EditorSceneManager.OpenScene(scenePath);

            Text[] texts = Object.FindObjectsByType<Text>(FindObjectsSortMode.None);
            foreach (Text t in texts)
            {
                Undo.RecordObject(t, "Replace Font");
                t.font = targetFont;
                EditorUtility.SetDirty(t);
            }
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        // 2. 모든 프리팹 처리
        string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in allPrefabGuids)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
            Text[] texts = prefabRoot.GetComponentsInChildren<Text>(true);

            bool changed = false;
            foreach (Text t in texts)
            {
                Undo.RecordObject(t, "Replace Font Prefab");
                t.font = targetFont;
                changed = true;
            }

            if (changed)
            {
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            }
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        Debug.Log($"모든 폰트가 {targetFont.name}으로 교체되었습니다.");
    }
}