using UnityEngine;
using UnityEngine.SceneManagement;

public class Okay : MonoBehaviour
{
    //근데 이제 입력을 다 했을 시 넘어가야함.
    public void GotoPlayScene()
    {   Debug.Log("메인 화면입니다.");
        SceneManager.LoadScene(3);
    }
}