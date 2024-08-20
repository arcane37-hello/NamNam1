using UnityEngine;
using UnityEngine.SceneManagement;

public class GoRefrigerator : MonoBehaviour
{
    //임시로 해놓았고 버튼있을때 쓰면 될듯 아직 화면이 안만들어져서 못 연결함.
    public void GotoPlayScene()
    {   Debug.Log("다음화면입니다.");
        SceneManager.LoadScene(1);
    }
}