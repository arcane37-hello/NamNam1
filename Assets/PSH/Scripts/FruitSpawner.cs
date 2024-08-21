using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class FruitPrefabProbability
{
    public GameObject prefab;
    public float probability; // ��ȯ Ȯ�� (0.0f ~ 1.0f)
}

public class FruitSpawner : MonoBehaviour
{
    // �����հ� �׿� ���� Ȯ���� �����ϴ� �迭
    public FruitPrefabProbability[] fruitPrefabs;

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
        // ��ȯ�� �������� Ȯ���� ���� �����մϴ�.
        GameObject selectedPrefab = GetRandomFruitPrefab();

        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No fruit prefab selected.");
        }
    }

    private GameObject GetRandomFruitPrefab()
    {
        float totalProbability = 0f;
        foreach (var fruit in fruitPrefabs)
        {
            totalProbability += fruit.probability;
        }

        float randomValue = Random.value * totalProbability;
        float cumulativeProbability = 0f;

        foreach (var fruit in fruitPrefabs)
        {
            cumulativeProbability += fruit.probability;
            if (randomValue <= cumulativeProbability)
            {
                return fruit.prefab;
            }
        }

        return null; // �⺻������ null ��ȯ (Ȯ���� 0�� ��츦 ���)
    }
}