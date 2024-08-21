using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public Transform targetPosition;  // ī�޶��� ��ǥ ��ġ�� ȸ��
    public float duration = 3.0f;     // �̵��� �ɸ��� �ð�
    public Camera mainCamera;         // �̵��� ī�޶� (�ν����Ϳ��� �Ҵ�)
    public Button moveButton;         // �̵��� ������ ��ư (�ν����Ϳ��� �Ҵ�)
    public Transform objectToRotate;  // ȸ����ų ������Ʈ (�ν����Ϳ��� �Ҵ�)
    public float rotationDuration = 1.0f; // ȸ���ϴ� �� �ɸ��� �ð�
    public float rotationAngle = 90.0f;  // ȸ���� ����

    private bool isMoving = false;    // ī�޶� �̵� ���� �÷���
    private Coroutine moveCoroutine;  // ī�޶� �̵� �ڷ�ƾ �ڵ�
    private Coroutine rotateCoroutine; // ȸ�� �ڷ�ƾ �ڵ�

    void Start()
    {
        // ��ư�� �Ҵ���� �ʾ����� ���� �޽��� ���
        if (moveButton != null)
        {
            // ��ư Ŭ�� �� ī�޶� �̵� �� ������Ʈ ȸ�� ����
            moveButton.onClick.AddListener(OnMoveButtonClick);
        }
        else
        {
            Debug.LogError("Move Button�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // ī�޶�� ��ǥ ��ġ�� �Ҵ���� �ʾ����� ���� �޽��� ���
        if (mainCamera == null)
        {
            Debug.LogError("���� ī�޶� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (targetPosition == null)
        {
            Debug.LogError("Ÿ�� ��ġ�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (objectToRotate == null)
        {
            Debug.LogError("ȸ����ų ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    public void OnMoveButtonClick()
    {
        if (!isMoving)
        {
            // ī�޶� �̵� �߿��� ��ư ��Ȱ��ȭ
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

        // �̵� �Ϸ� �� ��Ȯ�� ��ġ�� ȸ�� ����
        mainCamera.transform.position = targetPosition.position;
        mainCamera.transform.rotation = targetPosition.rotation;

        // �̵� �Ϸ� �� ��ư �ٽ� Ȱ��ȭ
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
            yield break; // ȸ����ų ������Ʈ�� ������ �ڷ�ƾ ����
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

        // ���� ȸ���� ����
        objectToRotate.eulerAngles = new Vector3(objectToRotate.eulerAngles.x, endRotationY, objectToRotate.eulerAngles.z);
    }
}