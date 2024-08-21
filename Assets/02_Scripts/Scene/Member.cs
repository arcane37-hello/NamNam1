using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Member : MonoBehaviour
{
    public AudioClip transitionSound;  // 재생할 사운드 클립
    private AudioSource audioSource;    // AudioSource 컴포넌트

    private void Start()
    {
        // AudioSource 컴포넌트를 가져옵니다. 없으면 추가합니다.
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void GotoPlayScene()
    {
        Debug.Log("회원가입 화면입니다.");

        // 사운드를 재생합니다.
        if (transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }

        // 사운드 재생이 끝난 후 씬 전환
        Invoke("LoadScene", transitionSound.length);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(1);  // 씬 인덱스 1으로 전환
        // 또는 SceneManager.LoadScene("PlayScene");  // 씬 이름으로 전환
    }
}