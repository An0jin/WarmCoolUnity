using System.IO; // 파일 시스템 조작(파일 삭제 등)을 위한 System.IO 네임스페이스 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.SceneManagement; // Unity 씬전환 네임스페이스 참조
using Toneiverse; // 프로젝트 네임스페이스 참조

/// <summary>로컬 인증 정보와 세션을 비운 뒤 로그인 씬으로 이동합니다.</summary>
public class LogOutBtn : SceneBtn // SceneBtn을 상속받는 로그아웃 전용 버튼 클래스
{
    // 클릭 이벤트 처리 메서드 재정의
    protected override void OnClick()
    {
        File.Delete(Env.I.Config.FilePath); // 로컬 기기에 저장된 로그인 토큰 파일 삭제
        Session.session.LogOut(); // 메모리 상의 사용자 세션 데이터 및 탐색 기록 초기화
        base.OnClick(); // 부모 클래스의 OnClick()을 호출하여 지정된 씬으로 이동
    }
}
