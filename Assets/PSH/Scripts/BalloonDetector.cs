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

        // balloon�� ��Ȱ��ȭ�� ��쿡�� FruitSpawner�� Ȱ��ȭ�ϵ��� ��û�մϴ�.
        if (!balloon.activeSelf)
        {
            fruitSpawner.ActivateForDuration();
        }
    }
}