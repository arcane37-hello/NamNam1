using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject[] spawners; // ������ �迭

    private void Start()
    {
        // ��� �����ʸ� ��Ȱ��ȭ�մϴ�.
        DeactivateAllSpawners();
    }

    public void ActivateSpawner(int index, float activationDuration)
    {
        if (index < 0 || index >= spawners.Length) return;

        StartCoroutine(ActivateAndDeactivateSpawner(index, activationDuration));
    }

    private IEnumerator ActivateAndDeactivateSpawner(int index, float activationDuration)
    {
        // �����ʸ� Ȱ��ȭ�մϴ�.
        spawners[index].SetActive(true);

        // ������ �ð� ���� Ȱ��ȭ ���¸� �����մϴ�.
        yield return new WaitForSeconds(activationDuration);

        // �����ʸ� ��Ȱ��ȭ�մϴ�.
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