using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;


[System.Serializable]
public class LoginRequest
{
    public string uuid;
}
public class LoginManager : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    private const string LOGIN_URL = "http://172.16.17.131:8080/api/v1/users/login";

    private void Start()
    {
        if (loginButton != null)
        {
            loginButton.onClick.AddListener(OnLoginButtonClick);
        }
    }

    private void OnLoginButtonClick()
    {
        string uuid = UUIDManager.GetUUID();
        StartCoroutine(SendLoginRequest(uuid));
    }

    private IEnumerator SendLoginRequest(string uuid)
    {
        LoginRequest loginRequest = new LoginRequest { uuid = uuid };
        string jsonBody = JsonUtility.ToJson(loginRequest);

        using (UnityWebRequest www = new UnityWebRequest(LOGIN_URL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                // �α��� ���� �� ȸ������ ȭ������ �̵�
                SceneManager.LoadScene("SignUp");
            }
            else
            {
                // �α��� ���� ó��
                // ���� ���� ������ �̵��ϰų� �ٸ� ó���� ����
                SceneManager.LoadScene("Kitchen");
            }
        }
    }
}