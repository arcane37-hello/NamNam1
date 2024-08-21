using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class SSEClient : MonoBehaviour
{
    [SerializeField] private string sseEndpoint = "http://172.16.17.131:8000/events";
    [SerializeField] private float reconnectInterval = 5f;
    [SerializeField] private int maxReconnectAttempts = 5;

    private void Start()
    {
        StartCoroutine(ReceiveServerSentEvents());
    }

    private IEnumerator ReceiveServerSentEvents()
    {
        int reconnectAttempts = 0;

        while (reconnectAttempts < maxReconnectAttempts)
        {
            Debug.Log($"SSE ���� �õ� ��... (�õ� {reconnectAttempts + 1}/{maxReconnectAttempts})");

            using (UnityWebRequest request = UnityWebRequest.Get(sseEndpoint))
            {
                request.SetRequestHeader("Accept", "text/event-stream");
                request.downloadHandler = new StreamingDownloadHandler();
                request.timeout = 30;

                yield return request.SendWebRequest();

                Debug.Log($"���� ����: {request.result}");

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"���� ����: {request.error}");
                    reconnectAttempts++;
                    yield return new WaitForSeconds(reconnectInterval);
                }
                else
                {
                    Debug.Log($"���� ����! ���� �ڵ�: {request.responseCode}");
                    reconnectAttempts = 0;
                    StreamingDownloadHandler downloadHandler = (StreamingDownloadHandler)request.downloadHandler;
                    while (!downloadHandler.isDone)
                    {
                        List<string> events = downloadHandler.GetEvents();
                        foreach (string eventData in events)
                        {
                            ProcessEvent(eventData);
                        }
                        yield return null;
                    }
                }
            }
        }

        Debug.LogWarning("�ִ� �翬�� �õ� Ƚ�� �ʰ�. SSE ���� ����.");
    }

    private void ProcessEvent(string eventData)
    {
        // ���⿡�� �̺�Ʈ �����͸� ó���մϴ�.
        Debug.Log($"���ŵ� �̺�Ʈ: {eventData}");
    }
}

public class StreamingDownloadHandler : DownloadHandlerScript
{
    private StringBuilder buffer = new StringBuilder();
    private List<string> events = new List<string>();
    private bool _isDone = false;

    public bool isDone => _isDone;

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (data == null || data.Length < 1)
        {
            return false;
        }

        string chunk = Encoding.UTF8.GetString(data, 0, dataLength);
        buffer.Append(chunk);

        // ������ �̺�Ʈ�� ã�� ó���մϴ�.
        int index;
        while ((index = buffer.ToString().IndexOf("\n\n")) != -1)
        {
            string eventData = buffer.ToString(0, index).Trim();
            events.Add(eventData);
            buffer.Remove(0, index + 2);
        }

        return true;
    }

    public List<string> GetEvents()
    {
        List<string> result = new List<string>(events);
        events.Clear();
        return result;
    }

    protected override void CompleteContent()
    {
        _isDone = true;
        Debug.Log("SSE ��Ʈ�� ����");
    }
}