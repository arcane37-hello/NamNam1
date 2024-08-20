using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    // 소환할 프리팹을 지정하는 public 변수
    public GameObject fruitPrefab;

    // 프리팹을 소환할 주기를 설정하는 public 변수
    public float spawnInterval = 2.0f; // 예: 2초마다 소환

    // 내부 타이머 변수
    private float timer;

    private void Start()
    {
        // 타이머를 초기화합니다.
        timer = spawnInterval;
    }

    private void Update()
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

    private void SpawnFruit()
    {
        // 현재 오브젝트의 위치에 프리팹을 소환합니다.
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