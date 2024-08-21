using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDetector : MonoBehaviour
{
    public GameObject balloon; // 확인할 풍선
    public SpawnerController spawnerController; // 스포너 컨트롤러
    public int spawnerIndex; // 연결된 스포너의 인덱스

    private void Update()
    {
        if (balloon == null || spawnerController == null) return;

        // 풍선이 비활성화된 경우에만 스포너를 활성화하도록 요청합니다.
        if (!balloon.activeSelf)
        {
            spawnerController.ActivateSpawner(spawnerIndex, 2.5f);
        }
    }
}