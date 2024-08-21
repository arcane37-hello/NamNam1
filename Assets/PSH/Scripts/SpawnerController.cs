using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject[] spawners; // 스포너 배열

    private void Start()
    {
        // 모든 스포너를 비활성화합니다.
        DeactivateAllSpawners();
    }

    public void ActivateSpawner(int index, float activationDuration)
    {
        if (index < 0 || index >= spawners.Length) return;

        StartCoroutine(ActivateAndDeactivateSpawner(index, activationDuration));
    }

    private IEnumerator ActivateAndDeactivateSpawner(int index, float activationDuration)
    {
        // 스포너를 활성화합니다.
        spawners[index].SetActive(true);

        // 지정된 시간 동안 활성화 상태를 유지합니다.
        yield return new WaitForSeconds(activationDuration);

        // 스포너를 비활성화합니다.
        spawners[index].SetActive(false);
    }

    public void DeactivateAllSpawners()
    {
        foreach (var spawner in spawners)
        {
            spawner.SetActive(false);
        }
    }
}