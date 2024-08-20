using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text.Json;

[Serializable]
public struct PostInfo
{
    public string uuid;
    public string date;
    public bool special;
}

[Serializable]
public struct PostInfoArray
{
    public PostInfo[] data;
}

public class HttpInfo
{
    public string url = "";
    public string body = "";
    public string contentType = "";
    public Action<DownloadHandler> onComplete;
}

public class HttpManager : MonoBehaviour
{
    private static HttpManager instance;

    public static HttpManager GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("HttpManager");
            instance = go.AddComponent<HttpManager>();
            DontDestroyOnLoad(go);
        }
        return instance;
    }

    public IEnumerator Get(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(info.url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                info.onComplete?.Invoke(webRequest.downloadHandler);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
    }

    public IEnumerator Post(HttpInfo info)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(info.body);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", info.contentType);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                info.onComplete?.Invoke(webRequest.downloadHandler);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
    }

    public IEnumerator PostJson<T>(string url, T data, Action<DownloadHandler> onComplete)
    {
        string json = JsonUtility.ToJson(data);
        HttpInfo info = new HttpInfo
        {
            url = url,
            body = json,
            contentType = "application/json",
            onComplete = onComplete
        };

        return Post(info);
    }
}