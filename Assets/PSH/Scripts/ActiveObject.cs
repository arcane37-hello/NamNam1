using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : MonoBehaviour
{
    public GameObject[] balloons; // ǳ�� �迭
    public SpawnerController spawnerController; // ������ ��Ʈ�ѷ�
    public float activationDuration = 2.5f; // ������ Ȱ��ȭ �ð�

    private int currentIndex = 0; // ���� Ȱ��ȭ�� ǳ���� �ε���

    void Start()
    {
        // �迭�� ��� ǳ���� ��Ȱ��ȭ�ϰ� ù ��° ǳ���� Ȱ��ȭ
        if (balloons.Length > 0)
        {
            for (int i = 0; i < balloons.Length; i++)
            {
                balloons[i].SetActive(false);
            }
            balloons[currentIndex].SetActive(true);
        }

        // �����ʵ��� ��Ȱ��ȭ�մϴ�.
        if (spawnerController != null)
        {
            spawnerController.DeactivateAllSpawners();
        }
    }

    void Update()
    {
        // ���� Ȱ��ȭ�� ǳ���� ��Ȱ��ȭ�Ǹ� ���� ǳ���� Ȱ��ȭ
        if (currentIndex < balloons.Length && !balloons[currentIndex].activeSelf)
        {
            // ���� ǳ���� ��Ȱ��ȭ
            balloons[currentIndex].SetActive(false);

            // �����ʸ� Ȱ��ȭ
            if (spawnerController != null)
            {
                spawnerController.ActivateSpawner(currentIndex, activationDuration);
            }

            // ���� ǳ���� �ε��� ���
            currentIndex++;

            // ���� ǳ���� ������ Ȱ��ȭ
            if (currentIndex < balloons.Length)
            {
                balloons[currentIndex].SetActive(true);
            }
        }
    }
}