using UnityEngine.Networking;
using UnityEngine;

public class HttpGetter : MonoBehaviour
{
    public PostInfoArray allPostInfo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FetchData();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SendData();
        }
    }

    private void FetchData()
    {
        HttpInfo info = new HttpInfo
        {
            url = "http://172.16.17.180:8000/chewing_count",
            onComplete = (DownloadHandler downloadHandler) =>
            {
                Debug.Log("Received data: " + downloadHandler.text);
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                allPostInfo = JsonUtility.FromJson<PostInfoArray>(jsonData);
                Debug.Log("Data fetched and converted to PostInfoArray");
            }
        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }

    private void SendData()
    {
        if (allPostInfo.data != null && allPostInfo.data.Length > 0)
        {
            string url = "http://172.16.17.131:8080/swagger-ui/index.html"; // Replace with your actual API URL
            StartCoroutine(HttpManager.GetInstance().PostJson(url, allPostInfo, OnSendComplete));
        }
        else
        {
            Debug.LogWarning("No data to send. Fetch data first.");
        }
    }

    private void OnSendComplete(DownloadHandler downloadHandler)
    {
        Debug.Log("Send operation completed. Server response: " + downloadHandler.text);
    }
}