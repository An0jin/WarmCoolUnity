using System.IO; // 로컬 토큰 파일 삭제를 위한 System.IO 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.SceneManagement; // SceneManager 씬 전환 참조
using UnityEngine.UI; // UI 시스템 참조

/// <summary>회원 정보를 삭제하고 로컬 로그인 파일을 정리합니다.</summary>
public class DeleteBtn : MSGBtn // 회원 탈퇴 요청 버튼 처리 스크립트
{
    bool isDelete; // 중복 클릭 차단 플래그

    // 초기화 생명주기
    protected override void Awake()
    {
        isDelete = true;
        base.Awake();
    }

    // 탈퇴 클릭 핸들러
    protected override void OnClick()
    {
        if (isDelete)
        {
            isDelete = false; // 진행 중 플래그 변경

            // API "user/{token}" DELETE 요청 전송
            StartCoroutine(APIManager.Delete($"user/{Session.session.Token}", (success) =>
            {
                File.Delete(Env.I.Config.FilePath); // 로컬 저장 토큰 파일 완전 삭제
                SceneManager.LoadScene(0); // 타이틀/로그인 씬(인덱스 0)으로 복귀
            },
            (err) =>
            {
                Error("삭제 실패. (서버 연결 오류)");
                isDelete = true; // 오류 시 재시도 허용
            }));
        }
    }
}
