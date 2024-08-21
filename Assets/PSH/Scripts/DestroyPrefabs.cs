using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabs : MonoBehaviour
{
    // �� ��ũ��Ʈ�� ������ ������Ʈ�� Ʈ���ŵ� ������Ʈ�� �ı��� �� �ֵ��� �մϴ�.
    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���ŵ� ������Ʈ�� �������� ���ɴϴ�.
        GameObject hitObject = other.gameObject;

        // Ʈ���ŵ� ������Ʈ�� Ư�� �±׸� ���� ������Ʈ���� Ȯ���մϴ�.
        if (hitObject.CompareTag("Fruit"))
        {
            // ������Ʈ�� �ı��մϴ�.
            Destroy(hitObject);
        }
    }
}