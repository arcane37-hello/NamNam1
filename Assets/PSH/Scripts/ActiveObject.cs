using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : MonoBehaviour
{
    public GameObject[] balloons; // 풍선 배열
    public SpawnerController spawnerController; // 스포너 컨트롤러
    public float activationDuration = 2.5f; // 스포너 활성화 시간

    private int currentIndex = 0; // 현재 활성화된 풍선의 인덱스

    void Start()
    {
        // 배열의 모든 풍선을 비활성화하고 첫 번째 풍선만 활성화
        if (balloons.Length > 0)
        {
            for (int i = 0; i < balloons.Length; i++)
            {
                balloons[i].SetActive(false);
            }
            balloons[currentIndex].SetActive(true);
        }

        // 스포너들을 비활성화합니다.
        if (spawnerController != null)
        {
            spawnerController.DeactivateAllSpawners();
        }
    }

    void Update()
    {
        // 현재 활성화된 풍선이 비활성화되면 다음 풍선을 활성화
        if (currentIndex < balloons.Length && !balloons[currentIndex].activeSelf)
        {
            // 현재 풍선을 비활성화
            balloons[currentIndex].SetActive(false);

            // 스포너를 활성화
            if (spawnerController != null)
            {
                spawnerController.ActivateSpawner(currentIndex, activationDuration);
            }

            // 다음 풍선의 인덱스 계산
            currentIndex++;

            // 다음 풍선이 있으면 활성화
            if (currentIndex < balloons.Length)
            {
                balloons[currentIndex].SetActive(true);
            }
        }
    }
}