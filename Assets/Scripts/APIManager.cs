using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // UnityWebRequest 등 웹 네트워크 통신 클래스 제공
using System.Collections; // IEnumerator 코루틴 인터페이스 제공
using System; // Action 콜백 델리게이트 등 기본 닷넷 타입 제공
using System.Text; // 텍스트 인코딩 관련 클래스 제공

/// <summary>HTTP 요청을 생성하고 서버 응답을 성공/실패 콜백으로 전달합니다.</summary>
public static class APIManager // 모든 HTTP 요청 API를 정적으로 제공하는 클래스
{
    // [핵심] 모든 요청 전송 및 성공/실패 응답 처리를 공통 담당하는 비공개 코루틴
    private static IEnumerator SendRequest(UnityWebRequest www, Action<string> onSuccess, Action<string> onError)
    {
        // www 객체의 통신 리소스를 안전하게 메모리 해제하기 위한 using 블록
        using (www)
        {
            // 네트워크 통신 요청을 보내고 응답 수신까지 대기
            yield return www.SendWebRequest();

            // 요청 결과가 성공인지 판별
            if (www.result == UnityWebRequest.Result.Success)
            {
                // 응답 본문 텍스트 추출 (null일 경우 빈 문자열 처리)
                string data = www.downloadHandler?.text ?? "";
                onSuccess?.Invoke(data); // 성공 콜백 호출
            }
            else
            {
                onError?.Invoke(www.error); // 네트워크 또는 HTTP 에러 발생 시 에러 콜백 호출
            }
        }
    }

    // GET 요청 실행 메서드
    public static IEnumerator Get(string endpoint, Action<string> onSuccess = null, Action<string> onError = null)
    {
        // URL을 조합하여 UnityWebRequest GET 요청 생성
        UnityWebRequest www = UnityWebRequest.Get(Env.I.Config.Api(endpoint));
        yield return SendRequest(www, onSuccess, onError); // 공통 전송 루틴 호출
    }

    // POST 요청 실행 메서드 (WWWForm 데이터 전송 가능)
    public static IEnumerator Post(string endpoint, WWWForm form = null, Action<string> onSuccess = null, Action<string> onError = null)
    {
        // 폼 데이터를 담아 UnityWebRequest POST 요청 생성
        UnityWebRequest www = UnityWebRequest.Post(Env.I.Config.Api(endpoint), form);
        yield return SendRequest(www, onSuccess, onError); // 공통 전송 루틴 호출
    }

    // PUT 요청 실행 메서드 (JSON 데이터 전송)
    public static IEnumerator Put(string endpoint, string json, Action<string> onSuccess = null, Action<string> onError = null)
    {
        // JSON 문자열 데이터를 전송하는 UnityWebRequest PUT 요청 생성
        UnityWebRequest www = UnityWebRequest.Put(Env.I.Config.Api(endpoint), json);
        www.SetRequestHeader("Content-Type", "application/json"); // JSON 요청 헤더 명시
        yield return SendRequest(www, onSuccess, onError); // 공통 전송 루틴 호출
    }

    // DELETE 요청 실행 메서드
    public static IEnumerator Delete(string endpoint, Action<string> onSuccess = null, Action<string> onError = null)
    {
        // UnityWebRequest DELETE 요청 생성
        UnityWebRequest www = UnityWebRequest.Delete(Env.I.Config.Api(endpoint));
        www.downloadHandler = new DownloadHandlerBuffer(); // 서버의 텍스트 응답을 수신하기 위한 버퍼 생성
        yield return SendRequest(www, onSuccess, onError); // 공통 전송 루틴 호출
    }
}
