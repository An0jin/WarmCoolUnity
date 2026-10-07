using System; // Exception, OperationCanceledException 참조
using System.Collections.Generic; // List 제네릭 컬렉션 제공
using System.Threading; // CancellationToken 참조
using UnityEngine; // Unity 기본 엔진 네임스페이스 참조
using UnityEngine.Networking; // UnityWebRequest 등 웹 네트워크 통신 클래스 제공

/// <summary>HTTP 요청을 생성하고 서버 응답을 UnityEngine.Awaitable 비동기 패턴으로 처리합니다.</summary>
public static class APIManager
{
    /// <summary>공통 비동기 요청 전송 메서드 (Zero-allocation 연동)</summary>
    private static async Awaitable<string> SendRequestAsync(UnityWebRequest www)
    {
        using (www)
        {

            await www.SendWebRequest();
            return www.result == UnityWebRequest.Result.Success
                ? www.downloadHandler?.text ?? string.Empty
                : throw new UnityException($"[APIManager] HTTP {www.responseCode} Error: {www.error}\n{www.downloadHandler?.text}");
        }
    }

    /// <summary>GET 요청 비동기 실행 (Awaitable)</summary>
    public static async Awaitable<string> GetAsync(string endpoint)
    {
        UnityWebRequest www = UnityWebRequest.Get(Env.I.Config.Api(endpoint));
        return await SendRequestAsync(www);
    }

    /// <summary>GET 요청 및 JSON DTO 역직렬화 (Awaitable)</summary>
    public static async Awaitable<T> GetJsonAsync<T>(string endpoint)
    {
        string jsonText = await GetAsync(endpoint);
        return JsonUtility.FromJson<T>(jsonText);
    }

    /// <summary>POST 요청 비동기 실행 (Awaitable)</summary>
    public static async Awaitable<string> PostAsync(string endpoint, List<IMultipartFormSection> form = null)
    {
        UnityWebRequest www = (form != null && form.Count > 0)
            ? UnityWebRequest.Post(Env.I.Config.Api(endpoint), form)
            : UnityWebRequest.Post(Env.I.Config.Api(endpoint), new List<IMultipartFormSection>());
        return await SendRequestAsync(www);
    }

    /// <summary>POST 요청 및 JSON DTO 역직렬화 (Awaitable)</summary>
    public static async Awaitable<T> PostJsonAsync<T>(string endpoint, List<IMultipartFormSection> form = null)
    {
        string jsonText = await PostAsync(endpoint, form);
        return JsonUtility.FromJson<T>(jsonText);
    }

    /// <summary>PUT 요청 비동기 실행 (Awaitable, JSON 바디)</summary>
    public static async Awaitable<string> PutAsync(string endpoint, string json)
    {
        UnityWebRequest www = UnityWebRequest.Put(Env.I.Config.Api(endpoint), json);
        www.SetRequestHeader("Content-Type", "application/json");
        return await SendRequestAsync(www);
    }

    /// <summary>DELETE 요청 비동기 실행 (Awaitable)</summary>
    public static async Awaitable<string> DeleteAsync(string endpoint)
    {
        UnityWebRequest www = UnityWebRequest.Delete(Env.I.Config.Api(endpoint));
        www.downloadHandler = new DownloadHandlerBuffer();
        return await SendRequestAsync(www);
    }
}
