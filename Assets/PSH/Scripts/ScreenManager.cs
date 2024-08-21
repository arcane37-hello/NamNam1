using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Button rightButton; // 오른쪽 버튼
    public Button leftButton;  // 왼쪽 버튼
    public Transform rightTarget; // 오른쪽 버튼 클릭 시 카메라가 이동할 위치
    public Transform leftTarget;  // 왼쪽 버튼 클릭 시 카메라가 이동할 위치
    public float moveDuration = 1.0f; // 카메라 이동에 걸리는 시간

    private void Start()
    {
        // 버튼 클릭 이벤트 등록
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

        // 카메라를 부드럽게 이동시키기
        while (elapsedTime < moveDuration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 목표 위치에 정확히 도달하도록 설정
        Camera.main.transform.position = targetPosition;
    }
}