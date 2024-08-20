using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBigger : MonoBehaviour
{
    public float scaleIncrement = 0.1f;  // ũ�⸦ ������ų ��
    private int keyPressCount = 0;       // J Ű�� ���� Ƚ���� �����ϴ� ����

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))  // J Ű�� ������ ��
        {
            keyPressCount++;  // Ű �Է� Ƚ�� ����

            // ������Ʈ�� ũ�⸦ ������Ŵ
            transform.localScale += new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

            // J Ű�� 30�� ������ ������Ʈ �ı�
            if (keyPressCount >= 30)
            {
                Destroy(gameObject);
            }
        }
    }
}