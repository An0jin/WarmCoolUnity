using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Text;

public static class APIManager
{
    // [핵심] 모든 요청은 이 함수 하나로 통한다 (중복 제거)
    private static IEnumerator SendRequest(UnityWebRequest www, Action<string> onSuccess, Action<string> onError)
    {
        using (www)
        {

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // 내용이 없으면(204 No Content 등) 빈 문자열 반환
                string data = www.downloadHandler?.text ?? "";
                onSuccess?.Invoke(data);
            }
            else
            {
                onError?.Invoke(www.error);
            }
        }
    }
    public static IEnumerator Get(string endpoint, Action<string> onSuccess = null, Action<string> onError = null)
    {
        UnityWebRequest www = UnityWebRequest.Get(Env.Api(endpoint));
        yield return SendRequest(www, onSuccess, onError);
    }
    public static IEnumerator Post(string endpoint, WWWForm form = null, Action<string> onSuccess = null, Action<string> onError = null)
    {
        UnityWebRequest www = UnityWebRequest.Post(Env.Api(endpoint), form);
        yield return SendRequest(www, onSuccess, onError);
    }
    public static IEnumerator Put(string endpoint, string json, Action<string> onSuccess = null, Action<string> onError = null)
    {
        UnityWebRequest www = UnityWebRequest.Put(Env.Api(endpoint), json);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return SendRequest(www, onSuccess, onError);
    }
    public static IEnumerator Delete(string endpoint, Action<string> onSuccess = null, Action<string> onError = null)
    {
        UnityWebRequest www = UnityWebRequest.Delete(Env.Api(endpoint));
        www.downloadHandler = new DownloadHandlerBuffer(); // Delete도 응답 본문이 있을 수 있음
        yield return SendRequest(www, onSuccess, onError);
    }
}