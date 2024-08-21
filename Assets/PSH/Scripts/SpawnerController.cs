using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject[] spawners; // 스포너 배열
    public Transform targetCameraPosition; // 카메라가 이동할 위치
    public GameObject objectToActivate; // 활성화할 특정 오브젝트
    public float cameraMoveDuration = 2.5f; // 카메라 이동 시간
    public float cameraMoveSpeed = 1.0f; // 카메라 이동 속도

    private void Start()
    {
        objectToActivate.SetActive(false);
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

        // 마지막 스포너가 비활성화된 후 카메라 이동 및 오브젝트 활성화
        if (index == spawners.Length - 1)
        {
            StartCoroutine(MoveCameraAndActivateObject());
        }
    }

    private IEnumerator MoveCameraAndActivateObject()
    {
        // 카메라의 현재 위치
        Vector3 startPosition = Camera.main.transform.position;

        // 카메라가 이동할 목표 위치
        Vector3 targetPosition = targetCameraPosition.position;

        float elapsedTime = 0f;

        // 카메라를 부드럽게 이동시킵니다.
        while (elapsedTime < cameraMoveDuration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / cameraMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 목표 위치에 정확히 도달하도록 설정
        Camera.main.transform.position = targetPosition;

        // 특정 오브젝트를 활성화합니다.
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