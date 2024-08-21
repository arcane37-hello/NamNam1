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
            Debug.Log($"SSE 연결 시도 중... (시도 {reconnectAttempts + 1}/{maxReconnectAttempts})");

            using (UnityWebRequest request = UnityWebRequest.Get(sseEndpoint))
            {
                // 요청 헤더 설정
                request.SetRequestHeader("Accept", "text/event-stream");
                request.downloadHandler = new StreamingDownloadHandler();
                request.timeout = 30;

                Debug.Log($"요청 URL: {sseEndpoint}");
                Debug.Log("설정된 요청 헤더:");
                Debug.Log($"  Accept: {request.GetRequestHeader("Accept")}");

                yield return request.SendWebRequest();

                Debug.Log($"연결 상태: {request.result}");

                // 응답 헤더 로깅
                Debug.Log("응답 헤더:");
                if (request.GetResponseHeaders() != null)
                {
                    foreach (var header in request.GetResponseHeaders())
                    {
                        Debug.Log($"  {header.Key}: {header.Value}");
                    }
                }
                else
                {
                    Debug.LogWarning("응답 헤더가 null입니다.");
                }

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"연결 오류: {request.error}");
                    Debug.LogError($"응답 코드: {request.responseCode}");
                    Debug.LogError($"상세 에러: {request.downloadHandler.error}");
                    reconnectAttempts++;
                    Debug.Log($"재연결 대기 중... ({reconnectInterval}초)");
                    yield return new WaitForSeconds(reconnectInterval);
                }
                else if (request.responseCode == 0)
                {
                    Debug.LogError("서버로부터 응답이 없습니다. 서버가 실행 중인지 확인하세요.");
                    reconnectAttempts++;
                    yield return new WaitForSeconds(reconnectInterval);
                }
                else
                {
                    Debug.Log($"연결 성공! 응답 코드: {request.responseCode}");
                    reconnectAttempts = 0;
                    StreamingDownloadHandler downloadHandler = (StreamingDownloadHandler)request.downloadHandler;
                    while (!downloadHandler.isDone)
                    {
                        string receivedData = downloadHandler.GetNextChunk();
                        if (!string.IsNullOrEmpty(receivedData))
                        {
                            Debug.Log($"수신된 데이터: {receivedData}");
                        }
                        yield return null;
                    }
                }
            }
        }

        Debug.LogWarning("최대 재연결 시도 횟수 초과. SSE 수신 종료.");
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
        Debug.Log($"데이터 청크 수신: {chunk}");
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
        Debug.Log("SSE 스트림 종료");
    }
}