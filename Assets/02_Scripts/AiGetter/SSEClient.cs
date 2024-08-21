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
                // ��û ��� ����
                request.SetRequestHeader("Accept", "text/event-stream");
                request.downloadHandler = new StreamingDownloadHandler();
                request.timeout = 30;

                Debug.Log($"��û URL: {sseEndpoint}");
                Debug.Log("������ ��û ���:");
                Debug.Log($"  Accept: {request.GetRequestHeader("Accept")}");

                yield return request.SendWebRequest();

                Debug.Log($"���� ����: {request.result}");

                // ���� ��� �α�
                Debug.Log("���� ���:");
                if (request.GetResponseHeaders() != null)
                {
                    foreach (var header in request.GetResponseHeaders())
                    {
                        Debug.Log($"  {header.Key}: {header.Value}");
                    }
                }
                else
                {
                    Debug.LogWarning("���� ����� null�Դϴ�.");
                }

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"���� ����: {request.error}");
                    Debug.LogError($"���� �ڵ�: {request.responseCode}");
                    Debug.LogError($"�� ����: {request.downloadHandler.error}");
                    reconnectAttempts++;
                    Debug.Log($"�翬�� ��� ��... ({reconnectInterval}��)");
                    yield return new WaitForSeconds(reconnectInterval);
                }
                else if (request.responseCode == 0)
                {
                    Debug.LogError("�����κ��� ������ �����ϴ�. ������ ���� ������ Ȯ���ϼ���.");
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
                        string receivedData = downloadHandler.GetNextChunk();
                        if (!string.IsNullOrEmpty(receivedData))
                        {
                            Debug.Log($"���ŵ� ������: {receivedData}");
                        }
                        yield return null;
                    }
                }
            }
        }

        Debug.LogWarning("�ִ� �翬�� �õ� Ƚ�� �ʰ�. SSE ���� ����.");
    }
}

public class StreamingDownloadHandler : DownloadHandlerScript
{
    private StringBuilder messageBuilder = new StringBuilder();
    private bool _isDone = false;

    public bool isDone => _isDone;

    public StreamingDownloadHandler() : base() { }

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (data == null || data.Length < 1)
        {
            return false;
        }

        string chunk = Encoding.UTF8.GetString(data, 0, dataLength);
        messageBuilder.Append(chunk);
        Debug.Log($"������ ûũ ����: {chunk}");
        return true;
    }

    public string GetNextChunk()
    {
        string message = messageBuilder.ToString();
        messageBuilder.Clear();
        return message;
    }

    protected override void CompleteContent()
    {
        _isDone = true;
        Debug.Log("SSE ��Ʈ�� ����");
    }
}