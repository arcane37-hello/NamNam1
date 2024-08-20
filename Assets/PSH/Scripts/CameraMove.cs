using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform targetPosition;  // 카메라가 이동할 목표 위치와 회전값을 가진 Transform
    public float duration = 3.0f;     // 이동에 걸리는 시간

    private bool isMoving = false;    // 카메라가 이동 중인지 체크하는 플래그

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭을 감지
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast로 클릭한 오브젝트 감지
            if (Physics.Raycast(ray, out hit))
            {
                // 클릭한 오브젝트가 이 스크립트가 붙어 있는 오브젝트인지 확인
                if (hit.transform == transform && !isMoving)
                {
                    StartCoroutine(MoveCameraToTarget());
                }
            }
        }
    }

    // 카메라를 목표 위치로 부드럽게 이동시키는 코루틴
    IEnumerator MoveCameraToTarget()
    {
        isMoving = true;

        Vector3 startPosition = Camera.main.transform.position;
        Quaternion startRotation = Camera.main.transform.rotation;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 위치와 회전을 부드럽게 보간
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / duration);
            Camera.main.transform.rotation = Quaternion.Lerp(startRotation, targetPosition.rotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;  // 다음 프레임까지 대기
        }

        // 마지막 위치와 회전을 목표 값으로 정확하게 설정
        Camera.main.transform.position = targetPosition.position;
        Camera.main.transform.rotation = targetPosition.rotation;

        isMoving = false;
    }
}