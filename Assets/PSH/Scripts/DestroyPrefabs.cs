using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabs : MonoBehaviour
{
    // �� ��ũ��Ʈ�� ������ ������Ʈ�� �浹�� �������� �ı��� �� �ֵ��� �մϴ�.
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �������� ���ɴϴ�.
        GameObject hitObject = collision.gameObject;

        // �浹�� ������Ʈ�� ���������� Ȯ���ϴ� ������ �߰��մϴ�.
        // ���� ���, Ư�� �±׸� ����Ͽ� Ȯ���� �� �ֽ��ϴ�.
        // �� ���������� "PrefabTag"��� �±׸� ���� ������Ʈ�� �ı��մϴ�.
        if (hitObject.CompareTag("Fruit"))
        {
            // ������Ʈ�� �ı��մϴ�.
            Destroy(hitObject);
        }
    }
}