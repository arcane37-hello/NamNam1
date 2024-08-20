using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    // 공기 저항이나 중력에 영향을 받지 않도록 할 수 있는 속도 변수입니다.
    public float fallSpeed = 5.0f;

    private Rigidbody rb;

    private void Start()
    {
        // Rigidbody 컴포넌트를 가져옵니다.
        rb = GetComponent<Rigidbody>();

        // Rigidbody가 없으면 경고 메시지를 출력합니다.
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody component is missing on this object.");
            return;
        }

        // 중력 영향을 끄고, 수동으로 이동 속도를 설정합니다.
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        // Rigidbody의 속도를 아래 방향으로 설정합니다.
        Vector3 fallDirection = new Vector3(0, -fallSpeed, 0);
        rb.velocity = fallDirection;
    }
}