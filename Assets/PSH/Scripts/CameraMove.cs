using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public Transform targetPosition;  // 카메라의 목표 위치와 회전
    public float duration = 3.0f;     // 이동에 걸리는 시간
    public Camera mainCamera;         // 이동할 카메라 (인스펙터에서 할당)
    public Button moveButton;         // 이동을 시작할 버튼 (인스펙터에서 할당)
    public Transform objectToRotate;  // 회전시킬 오브젝트 (인스펙터에서 할당)
    public float rotationDuration = 1.0f; // 회전하는 데 걸리는 시간
    public float rotationAngle = 90.0f;  // 회전할 각도

    private bool isMoving = false;    // 카메라 이동 상태 플래그
    private Coroutine moveCoroutine;  // 카메라 이동 코루틴 핸들
    private Coroutine rotateCoroutine; // 회전 코루틴 핸들

    void Start()
    {
        // 버튼이 할당되지 않았으면 오류 메시지 출력
        if (moveButton != null)
        {
            // 버튼 클릭 시 카메라 이동 및 오브젝트 회전 시작
            moveButton.onClick.AddListener(OnMoveButtonClick);
        }
        else
        {
            Debug.LogError("Move Button이 할당되지 않았습니다.");
        }

        // 카메라와 목표 위치가 할당되지 않았으면 오류 메시지 출력
        if (mainCamera == null)
        {
            Debug.LogError("메인 카메라가 할당되지 않았습니다.");
        }

        if (targetPosition == null)
        {
            Debug.LogError("타겟 위치가 할당되지 않았습니다.");
        }

        if (objectToRotate == null)
        {
            Debug.LogError("회전시킬 오브젝트가 할당되지 않았습니다.");
        }
    }

    public void OnMoveButtonClick()
    {
        if (!isMoving)
        {
            // 카메라 이동 중에는 버튼 비활성화
            if (moveButton != null)
            {
                moveButton.interactable = false;
            }

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveCameraToTarget());

            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            rotateCoroutine = StartCoroutine(RotateObjectSmoothly());
        }
    }

    IEnumerator MoveCameraToTarget()
    {
        isMoving = true;

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / duration);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetPosition.rotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동 완료 후 정확한 위치와 회전 설정
        mainCamera.transform.position = targetPosition.position;
        mainCamera.transform.rotation = targetPosition.rotation;

        // 이동 완료 후 버튼 다시 활성화
        if (moveButton != null)
        {
            moveButton.interactable = true;
        }

        isMoving = false;
    }

    IEnumerator RotateObjectSmoothly()
    {
        if (objectToRotate == null)
        {
            yield break; // 회전시킬 오브젝트가 없으면 코루틴 종료
        }

        float elapsedTime = 0.0f;
        float startRotationY = objectToRotate.eulerAngles.y;
        float endRotationY = startRotationY + rotationAngle;

        while (elapsedTime < rotationDuration)
        {
            float currentY = Mathf.Lerp(startRotationY, endRotationY, elapsedTime / rotationDuration);
            objectToRotate.eulerAngles = new Vector3(objectToRotate.eulerAngles.x, currentY, objectToRotate.eulerAngles.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 회전값 설정
        objectToRotate.eulerAngles = new Vector3(objectToRotate.eulerAngles.x, endRotationY, objectToRotate.eulerAngles.z);
    }
}