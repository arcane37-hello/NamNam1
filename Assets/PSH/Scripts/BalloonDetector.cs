using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDetector : MonoBehaviour
{
    public GameObject balloon;
    public FruitSpawner fruitSpawner;

    private void Update()
    {
        if (balloon == null || fruitSpawner == null) return;

        // balloon이 비활성화된 경우에만 FruitSpawner를 활성화하도록 요청합니다.
        if (!balloon.activeSelf)
        {
            fruitSpawner.ActivateForDuration();
        }
    }
}