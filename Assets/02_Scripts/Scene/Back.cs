using UnityEngine;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour
{
    public void GotoPlayScene()
    {   Debug.Log("선택화면입니다.");
        SceneManager.LoadScene(4);
    }
}