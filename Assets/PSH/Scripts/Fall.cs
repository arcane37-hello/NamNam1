using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    // ���� �����̳� �߷¿� ������ ���� �ʵ��� �� �� �ִ� �ӵ� �����Դϴ�.
    public float fallSpeed = 5.0f;

    private Rigidbody rb;

    private void Start()
    {
        // Rigidbody ������Ʈ�� �����ɴϴ�.
        rb = GetComponent<Rigidbody>();

        // Rigidbody�� ������ ��� �޽����� ����մϴ�.
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody component is missing on this object.");
            return;
        }

        // �߷� ������ ����, �������� �̵� �ӵ��� �����մϴ�.
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        // Rigidbody�� �ӵ��� �Ʒ� �������� �����մϴ�.
        Vector3 fallDirection = new Vector3(0, -fallSpeed, 0);
        rb.velocity = fallDirection;
    }
}