using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectBigger : MonoBehaviour
{
    public GameObject parent;            // ũ�⸦ ������ �θ� ������Ʈ
    public float scaleIncrement = 0.1f;  // ũ�⸦ ������ų ��
    private int keyPressCount = 0;       // J Ű�� ���� Ƚ���� �����ϴ� ����
    public Text countText;               // �ؽ�Ʈ ������Ʈ (�ν����Ϳ��� �Ҵ�)

    void Start()
    {
        if (countText != null)
        {
            // �ʱ� �ؽ�Ʈ�� 0���� ����
            countText.text = keyPressCount.ToString();
        }
        else
        {
            Debug.LogError("Count Text�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))  // J Ű�� ������ ��
        {
            keyPressCount++;  // Ű �Է� Ƚ�� ����

            // ������Ʈ�� ũ�⸦ ������Ŵ
            transform.localScale += new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

            // �ؽ�Ʈ�� ������Ʈ
            if (countText != null)
            {
                countText.text = keyPressCount.ToString();
            }

            // J Ű�� 30�� ������ �θ� ������Ʈ�� ��Ȱ��ȭ
            if (keyPressCount >= 30)
            {
                if (parent != null)
                {
                    parent.SetActive(false);  // �θ� ������Ʈ�� ��Ȱ��ȭ
                }
                else
                {
                    Debug.LogWarning("Parent GameObject is not assigned.");
                }
            }
        }
    }
}