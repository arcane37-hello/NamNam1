using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator animator;
    private bool open = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown() // 클릭 이벤트를 처리하는 메서드
    {
        if (!open) // 현재 문이 열려 있지 않은 상태일 때
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        open = true;
        animator.SetBool("open", open);
    }
}