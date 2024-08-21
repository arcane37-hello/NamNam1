using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject[] spawners; // ������ �迭
    public Transform targetCameraPosition; // ī�޶� �̵��� ��ġ
    public GameObject objectToActivate; // Ȱ��ȭ�� Ư�� ������Ʈ
    public float cameraMoveDuration = 2.5f; // ī�޶� �̵� �ð�
    public float cameraMoveSpeed = 1.0f; // ī�޶� �̵� �ӵ�

    private void Start()
    {
        objectToActivate.SetActive(false);
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

        // ������ �����ʰ� ��Ȱ��ȭ�� �� ī�޶� �̵� �� ������Ʈ Ȱ��ȭ
        if (index == spawners.Length - 1)
        {
            StartCoroutine(MoveCameraAndActivateObject());
        }
    }

    private IEnumerator MoveCameraAndActivateObject()
    {
        // ī�޶��� ���� ��ġ
        Vector3 startPosition = Camera.main.transform.position;

        // ī�޶� �̵��� ��ǥ ��ġ
        Vector3 targetPosition = targetCameraPosition.position;

        float elapsedTime = 0f;

        // ī�޶� �ε巴�� �̵���ŵ�ϴ�.
        while (elapsedTime < cameraMoveDuration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / cameraMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������� ��ǥ ��ġ�� ��Ȯ�� �����ϵ��� ����
        Camera.main.transform.position = targetPosition;

        // Ư�� ������Ʈ�� Ȱ��ȭ�մϴ�.
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }

    public void DeactivateAllSpawners()
    {
        foreach (var spawner in spawners)
        {
            spawner.SetActive(false);
        }
    }
}