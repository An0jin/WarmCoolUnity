using System.Collections; // IEnumerator 코루틴 관련 System.Collections 참조
using System.Collections.Generic; // List 제네릭 컬렉션 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // 네트워크 통신 관련 참조
using Toneiverse.DTO; // 통신 데이터 구조체 DTO 참조

/// <summary>진단 결과에 맞는 립스틱 목록을 불러와 색상 버튼을 생성합니다.</summary>
public class ColorView : MonoBehaviour // 사용자 진단 퍼스널컬러용 립스틱 목록 동적 생성 스크립트
{
    [SerializeField] ResultText cnameText; // 선택된 립스틱 제품명 표시 텍스트 필드

    // 첫 번째 프레임 업데이트 직전 호출
    void Start()
    {
        NavigationManager.navigationManager.ClearHistory(); // 씬 이력 스택 초기화

        // 세션의 ColorId(퍼스널컬러 그룹)에 대응하는 립스틱 리스트 GET 요청
        StartCoroutine(APIManager.Get($"/lipstick/{Session.session.ColorId}", (jsonText) =>
        {
            // 서버 립스틱 리스트 JSON 파싱
            JsonList<ColorJson> json = JsonUtility.FromJson<JsonList<ColorJson>>(jsonText);
            
            // 파싱된 각 립스틱 정보를 바탕으로 버튼 동적 인스턴스화
            foreach (var item in json.result)
            {
                // Resources/ColorBtn 프리팹 동적 생성
                ColorBtn btn = Instantiate(Resources.Load<ColorBtn>("ColorBtn"), transform);
                
                // 버튼의 색상과 제품명 및 UI 연동 설정
                btn.SetBtnColor(item.hex_code, item.cname, cnameText);
            }
        }));
    }
}
