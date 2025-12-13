using UnityEditor;
using UnityEngine;

// Editor 스크립트는 MonoBehaviour를 상속받을 필요가 없습니다. 단순 static 유틸리티 클래스로 선언하는 것이 더 적절합니다.
public class EditorResetPrefs
{
    // [수정됨] "Edit" 메뉴 하위에 "Reset Preferences"를 생성하도록 경로 지정
    [MenuItem("Edit/Reset Preferences")] 
    static void ResetPrefs()
    {
        if (EditorUtility.DisplayDialog("Reset editor preferences?", 
            "Reset all editor preferences? This cannot be undone.", "Yes", "No"))
        {
            EditorPrefs.DeleteAll();
            Debug.Log("Editor preferences have been reset."); // 실행 확인 로그 추가 권장
        }
    }
}