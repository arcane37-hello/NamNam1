using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    // ��ȯ�� �������� �����ϴ� public ����
    public GameObject fruitPrefab;

    // �������� ��ȯ�� �ֱ⸦ �����ϴ� public ����
    public float spawnInterval = 2.0f; // ��: 2�ʸ��� ��ȯ
    public float activeDuration = 3.0f; // Ȱ��ȭ �ð� (3��)

    // ���� Ÿ�̸� ����
    private float timer;
    private bool isActive = false;
    private bool shouldSpawn = false;

    private void Update()
    {
        if (isActive)
        {
            if (shouldSpawn)
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

    // ǳ���� ��Ȱ��ȭ�Ǿ��� �� ȣ��Ǵ� �޼���
    public void ActivateForDuration()
    {
        if (!isActive)
        {
            isActive = true;
            shouldSpawn = true;
            timer = spawnInterval; // Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
            StartCoroutine(DeactivateAfterDuration(activeDuration));
        }
    }

    private IEnumerator DeactivateAfterDuration(float duration)
    {
        // 3�� ���� Ȱ��ȭ ���� ����
        yield return new WaitForSeconds(duration);

        // ��ȯ ����� �����ϰ� ��Ȱ��ȭ
        shouldSpawn = false;
        isActive = false;
        gameObject.SetActive(false);
    }
}