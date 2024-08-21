using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    
    public void GotoPlayScene()
    {   Debug.Log("메인 화면입니다.");
        SceneManager.LoadScene(3);
    }
}