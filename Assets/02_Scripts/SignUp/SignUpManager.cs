using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class SignUpRequest
{
    public string uuid;
    public string userName;
    public int age;
}
public class SignUpManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private TMP_InputField ageInput;
    [SerializeField] private Button signUpButton;
    private const string SIGNUP_URL = "http://172.16.17.131:8080/api/v1/users/singup";

    private void Start()
    {
        if (signUpButton != null)
        {
            signUpButton.onClick.AddListener(OnSignUpButtonClick);
        }
        else
        {
            Debug.LogError("Sign Up Button is not assigned!");
        }
    }

    private void OnSignUpButtonClick()
    {
        string uuid = UUIDManager.GetUUID();
        string userName = userNameInput.text;
        int age;

        if (string.IsNullOrEmpty(userName) || !int.TryParse(ageInput.text, out age))
        {
            Debug.LogError("Invalid input. Please check user name and age.");
            return;
        }

        StartCoroutine(SendSignUpRequest(uuid, userName, age));
    }

    private IEnumerator SendSignUpRequest(string uuid, string userName, int age)
    {
        SignUpRequest signUpRequest = new SignUpRequest
        {
            uuid = uuid,
            userName = userName,
            age = age
        };
        string jsonBody = JsonUtility.ToJson(signUpRequest);

        using (UnityWebRequest www = new UnityWebRequest(SIGNUP_URL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Sign up failed: " + www.error);
            }
            else
            {
                Debug.Log("Sign up successful!");
                SceneManager.LoadScene("Login");
            }
        }
    }
}