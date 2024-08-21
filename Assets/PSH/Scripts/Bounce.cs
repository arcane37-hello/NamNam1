using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceForce = 10f;
    private bool isGrounded = false;

    void Update()
    {
        if (isGrounded)
        {
            // 여기에 논리를 추가하여 더 많은 행동을 정의할 수 있습니다.
            // 예: 튕기는 애니메이션 시작, 점프 버튼 입력 대기 등
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 지면과의 충돌을 확인합니다.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // 지면에서 떨어지면 isGrounded를 false로 설정합니다.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}