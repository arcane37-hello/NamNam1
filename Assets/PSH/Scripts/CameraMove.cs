using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// using System.Collections;

public class CameraMove : MonoBehaviour
{
    public Transform targetPosition;      // ī�޶��� ���� ��ǥ ��ġ�� ȸ��
    public Transform targetPosition2;     // ī�޶� ó������ �ε巴�� �̵��� ��ġ�� ȸ��
    public float moveDuration1 = 1.0f;    // targetPosition2�� �̵��ϴ� �� �ɸ��� �ð�
    public float moveDuration2 = 1.0f;    // targetPosition���� �̵��ϴ� �� �ɸ��� �ð�
    public Camera mainCamera;             // �̵��� ī�޶� (�ν����Ϳ��� �Ҵ�)
    public Button moveButton;             // �̵��� ������ ��ư (�ν����Ϳ��� �Ҵ�)
    public Transform objectToRotate;      // ȸ����ų ������Ʈ (�ν����Ϳ��� �Ҵ�)
    public float rotationDuration = 1.0f; // ȸ���ϴ� �� �ɸ��� �ð�
    public float rotationAngle = 90.0f;   // ȸ���� ����
    public Image fadeInImage;             // ���̵� ���� �̹��� (�ν����Ϳ��� �Ҵ�)
    public float fadeDuration = 1.0f;     // ���̵� ���ϴ� �� �ɸ��� �ð�

    private bool isMoving = false;        // ī�޶� �̵� ���� �÷���
    private Coroutine moveCoroutine;      // ī�޶� �̵� �ڷ�ƾ �ڵ�
    private Coroutine rotateCoroutine;    // ȸ�� �ڷ�ƾ �ڵ�

    void Start()
    {
        if (moveButton != null)
        {
            // ��ư Ŭ�� �� ī�޶� �̵� �� ������Ʈ ȸ�� ����
            moveButton.onClick.AddListener(OnMoveButtonClick);
        }
        else
        {
            Debug.LogError("Move Button�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (mainCamera == null)
        {
            Debug.LogError("���� ī�޶� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (targetPosition == null)
        {
            Debug.LogError("Ÿ�� ��ġ�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (targetPosition2 == null)
        {
            Debug.LogError("Ÿ�� ��ġ 2�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (objectToRotate == null)
        {
            Debug.LogError("ȸ����ų ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (fadeInImage == null)
        {
            Debug.LogError("���̵� ���� �̹����� �Ҵ���� �ʾҽ��ϴ�.");
        }
        else
        {
            // �̹����� ���� ���� 0���� �����Ͽ� ó������ ������ �ʵ��� �մϴ�.
            Color color = fadeInImage.color;
            color.a = 0;
            fadeInImage.color = color;
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
            moveCoroutine = StartCoroutine(MoveCameraSequence());
        }
    }

    IEnumerator MoveCameraSequence()
    {
        isMoving = true;

        // targetPosition2�� �ε巴�� �̵�
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        Vector3 targetPosition2Position = targetPosition2.position;
        Quaternion targetPosition2Rotation = targetPosition2.rotation;
        float elapsedTime = 0.0f;

        while (elapsedTime < moveDuration1)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition2Position, elapsedTime / moveDuration1);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetPosition2Rotation, elapsedTime / moveDuration1);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �̵� �Ϸ� �� ��Ȯ�� ��ġ�� ȸ�� ����
        mainCamera.transform.position = targetPosition2Position;
        mainCamera.transform.rotation = targetPosition2Rotation;

        // ���̵� �� �̹����� ���� ���� fadeDuration �ð� ���� �ε巴�� ����
        if (fadeInImage != null)
        {
            StartCoroutine(FadeInImage());
        }

        // ������Ʈ ȸ�� ����
        if (objectToRotate != null)
        {
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            rotateCoroutine = StartCoroutine(RotateObjectSmoothly());
        }

        // targetPosition���� �ε巴�� �̵�
        startPosition = mainCamera.transform.position;
        startRotation = mainCamera.transform.rotation;
        Vector3 targetPositionPosition = targetPosition.position;
        Quaternion targetPositionRotation = targetPosition.rotation;
        elapsedTime = 0.0f;

        while (elapsedTime < moveDuration2)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPositionPosition, elapsedTime / moveDuration2);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetPositionRotation, elapsedTime / moveDuration2);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �̵� �Ϸ� �� ��Ȯ�� ��ġ�� ȸ�� ����
        mainCamera.transform.position = targetPositionPosition;
        mainCamera.transform.rotation = targetPositionRotation;

        // �̵� �Ϸ� �� ��ư �ٽ� Ȱ��ȭ
        if (moveButton != null)
        {
            moveButton.interactable = true;
        }

        isMoving = false;
        SceneManager.LoadScene("MyRefrigerator");
    }

    IEnumerator FadeInImage()
    {
        float elapsedTime = 0.0f;
        Color startColor = fadeInImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // ���� ���� ��

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / fadeDuration);
            fadeInImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ���� �� ����
        fadeInImage.color = endColor;
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