using UnityEngine;
using UnityEngine.SceneManagement;

public class Member : MonoBehaviour
{
    
    public void GotoPlayScene()
    {   Debug.Log("회원가입 화면입니다.");
        SceneManager.LoadScene(1);
    }
}