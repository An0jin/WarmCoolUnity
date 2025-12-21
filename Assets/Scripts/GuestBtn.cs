using UnityEngine;
using UnityEngine.UI;

public class GuestBtn : SceneBtn
{

    // 부모의 Click 함수를 재정의(Override)
    protected override void OnClick()
    {
        // 1. 게스트 로그인 로직 실행
        // (오타가 있다면 isGeust 그대로, 수정했다면 isGuest로 사용)
        if (Session.session != null)
        {
            Session.session.isGuest = true;
            Debug.Log("게스트 모드로 설정됨");
        }
        else
        {
            Debug.LogError("Session 싱글톤이 초기화되지 않았습니다.");
        }

        // 2. 부모의 원래 기능(씬 이동) 실행
        base.OnClick();
    }
}
