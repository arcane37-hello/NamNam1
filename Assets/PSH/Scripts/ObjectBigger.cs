using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBigger : MonoBehaviour
{
    public float scaleIncrement = 0.1f;  // ũ�⸦ ������ų ��

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))  // J Ű�� ������ ��
        {
            // ������Ʈ�� ũ�⸦ ������Ŵ
            transform.localScale += new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

            // ���� ũ�Ⱑ 4�� �ʰ��ϸ� ������Ʈ �ı�
            if (transform.localScale.x > 4f || transform.localScale.y > 4f || transform.localScale.z > 4f)
            {
                Destroy(gameObject);
            }
        }
    }
}