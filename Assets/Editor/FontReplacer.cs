using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class FontReplacer : EditorWindow
{
    private Font targetFont;

    [MenuItem("Tools/Toneiverse/Replace All Fonts (Final)")]
    public static void ShowWindow()
    {
        GetWindow<FontReplacer>("Font Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Toneiverse 프로젝트 폰트 일괄 교체", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 교체할 폰트 선택 필드
        targetFont = (Font)EditorGUILayout.ObjectField("대상 폰트", targetFont, typeof(Font), false);

        if (GUILayout.Button("비활성 객체 포함 모든 폰트 교체"))
        {
            if (targetFont == null)
            {
                EditorUtility.DisplayDialog("오류", "교체할 폰트를 먼저 드래그 앤 드롭 하십시오.", "확인");
                return;
            }

            if (EditorUtility.DisplayDialog("주의", "프로젝트 내 모든 씬과 프리팹의 폰트가 교체됩니다. 계속하시겠습니까?", "예", "아니오"))
            {
                ExecuteReplace();
            }
        }
    }

    private void ExecuteReplace()
    {
        int sceneCount = 0;
        int prefabCount = 0;

        // 1. 모든 씬 처리 (Assets 폴더 내부만 필터링)
        string[] allSceneGuids = AssetDatabase.FindAssets("t:Scene");
        foreach (string guid in allSceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);

            // 읽기 전용 패키지 씬 무시 (기술적 결함 방지)
            if (!scenePath.StartsWith("Assets/")) continue;

            EditorSceneManager.OpenScene(scenePath);

            // 핵심 수정: 루트 객체부터 하위 비활성 객체까지 모두 검색
            List<Text> allTexts = new List<Text>();
            GameObject[] rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject root in rootObjects)
            {
                allTexts.AddRange(root.GetComponentsInChildren<Text>(true));
            }

            foreach (Text t in allTexts)
            {
                Undo.RecordObject(t, "Replace Font (All)");
                t.font = targetFont;
                EditorUtility.SetDirty(t);
            }

            if (allTexts.Count > 0)
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                sceneCount++;
            }
        }

        // 2. 모든 프리팹 처리 (Assets 폴더 내부만 필터링)
        string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in allPrefabGuids)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!prefabPath.StartsWith("Assets/")) continue;

            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
            // 비활성 자식 객체 포함 검색
            Text[] texts = prefabRoot.GetComponentsInChildren<Text>(true);

            if (texts.Length > 0)
            {
                foreach (Text t in texts)
                {
                    t.font = targetFont;
                }
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
                prefabCount++;
            }
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("완료", $"{sceneCount}개 씬, {prefabCount}개 프리팹 교체 완료", "확인");
        Debug.Log($"[Toneiverse] {targetFont.name}으로의 일괄 교체가 성공적으로 종료되었습니다.");
    }
}