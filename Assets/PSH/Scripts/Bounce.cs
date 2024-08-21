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
            // ���⿡ ���� �߰��Ͽ� �� ���� �ൿ�� ������ �� �ֽ��ϴ�.
            // ��: ƨ��� �ִϸ��̼� ����, ���� ��ư �Է� ��� ��
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // ������� �浹�� Ȯ���մϴ�.
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
        // ���鿡�� �������� isGrounded�� false�� �����մϴ�.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}