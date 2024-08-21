using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class FruitPrefabProbability
{
    public GameObject prefab;
    public float probability; // 소환 확률 (0.0f ~ 1.0f)
}

public class FruitSpawner : MonoBehaviour
{
    // 프리팹과 그에 대한 확률을 저장하는 배열
    public FruitPrefabProbability[] fruitPrefabs;

    // 프리팹을 소환할 주기를 설정하는 public 변수
    public float spawnInterval = 2.0f; // 예: 2초마다 소환
    public float activeDuration = 3.0f; // 활성화 시간 (3초)

    // 내부 타이머 변수
    private float timer;
    private bool isActive = false;
    private bool shouldSpawn = false;

    private void Update()
    {
        if (isActive)
        {
            if (shouldSpawn)
            {
                // 타이머를 감소시킵니다.
                timer -= Time.deltaTime;

                // 타이머가 0 이하일 때 소환을 수행합니다.
                if (timer <= 0f)
                {
                    SpawnFruit();
                    // 타이머를 리셋합니다.
                    timer = spawnInterval;
                }
            }
        }
    }

    private void SpawnFruit()
    {
        // 소환할 프리팹을 확률에 따라 선택합니다.
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

        return null; // 기본값으로 null 반환 (확률이 0인 경우를 대비)
    }

    // 활성화 상태를 설정하는 메서드
    public void ActivateForDuration()
    {
        if (!isActive)
        {
            isActive = true;
            shouldSpawn = true;
            timer = spawnInterval; // 타이머를 초기화합니다.
            StartCoroutine(DeactivateAfterDuration(activeDuration));
        }
    }

    private IEnumerator DeactivateAfterDuration(float duration)
    {
        // 활성화 상태를 유지합니다.
        yield return new WaitForSeconds(duration);

        // 소환 기능을 중지하고 비활성화
        shouldSpawn = false;
        isActive = false;
        gameObject.SetActive(false);
    }
}