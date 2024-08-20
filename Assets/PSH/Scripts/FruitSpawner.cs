using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    // ��ȯ�� �������� �����ϴ� public ����
    public GameObject fruitPrefab;

    // �������� ��ȯ�� �ֱ⸦ �����ϴ� public ����
    public float spawnInterval = 2.0f; // ��: 2�ʸ��� ��ȯ

    // ���� Ÿ�̸� ����
    private float timer;

    private void Start()
    {
        // Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
        timer = spawnInterval;
    }

    private void Update()
    {
        // Ÿ�̸Ӹ� ���ҽ�ŵ�ϴ�.
        timer -= Time.deltaTime;

        // Ÿ�̸Ӱ� 0 ������ �� ��ȯ�� �����մϴ�.
        if (timer <= 0f)
        {
            SpawnFruit();
            // Ÿ�̸Ӹ� �����մϴ�.
            timer = spawnInterval;
        }
    }

    private void SpawnFruit()
    {
        // ���� ������Ʈ�� ��ġ�� �������� ��ȯ�մϴ�.
        if (fruitPrefab != null)
        {
            Instantiate(fruitPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Fruit Prefab is not assigned in the FruitSpawner script.");
        }
    }
}