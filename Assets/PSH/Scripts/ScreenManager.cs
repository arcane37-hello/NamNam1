using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Button rightButton; // ������ ��ư
    public Button leftButton;  // ���� ��ư
    public Transform rightTarget; // ������ ��ư Ŭ�� �� ī�޶� �̵��� ��ġ
    public Transform leftTarget;  // ���� ��ư Ŭ�� �� ī�޶� �̵��� ��ġ
    public float moveDuration = 1.0f; // ī�޶� �̵��� �ɸ��� �ð�

    private void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ���
        if (rightButton != null)
        {
            rightButton.onClick.AddListener(OnRightButtonClicked);
        }

        if (leftButton != null)
        {
            leftButton.onClick.AddListener(OnLeftButtonClicked);
        }
    }

    public void OnRightButtonClicked()
    {
        StartCoroutine(MoveCamera(Camera.main.transform.position, rightTarget.position));
    }

    public void OnLeftButtonClicked()
    {
        StartCoroutine(MoveCamera(Camera.main.transform.position, leftTarget.position));
    }

    private IEnumerator MoveCamera(Vector3 startPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0f;

        // ī�޶� �ε巴�� �̵���Ű��
        while (elapsedTime < moveDuration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������� ��ǥ ��ġ�� ��Ȯ�� �����ϵ��� ����
        Camera.main.transform.position = targetPosition;
    }
}