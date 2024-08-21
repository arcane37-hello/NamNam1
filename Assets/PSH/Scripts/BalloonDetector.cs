using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDetector : MonoBehaviour
{
    public GameObject balloon; // Ȯ���� ǳ��
    public SpawnerController spawnerController; // ������ ��Ʈ�ѷ�
    public int spawnerIndex; // ����� �������� �ε���

    private void Update()
    {
        if (balloon == null || spawnerController == null) return;

        // ǳ���� ��Ȱ��ȭ�� ��쿡�� �����ʸ� Ȱ��ȭ�ϵ��� ��û�մϴ�.
        if (!balloon.activeSelf)
        {
            spawnerController.ActivateSpawner(spawnerIndex, 2.5f);
        }
    }
}