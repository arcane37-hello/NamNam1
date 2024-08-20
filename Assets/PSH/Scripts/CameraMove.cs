using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform targetPosition;  // ī�޶� �̵��� ��ǥ ��ġ�� ȸ������ ���� Transform
    public float duration = 3.0f;     // �̵��� �ɸ��� �ð�

    private bool isMoving = false;    // ī�޶� �̵� ������ üũ�ϴ� �÷���

    void Update()
    {
        // ���콺 ���� ��ư Ŭ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast�� Ŭ���� ������Ʈ ����
            if (Physics.Raycast(ray, out hit))
            {
                // Ŭ���� ������Ʈ�� �� ��ũ��Ʈ�� �پ� �ִ� ������Ʈ���� Ȯ��
                if (hit.transform == transform && !isMoving)
                {
                    StartCoroutine(MoveCameraToTarget());
                }
            }
        }
    }

    // ī�޶� ��ǥ ��ġ�� �ε巴�� �̵���Ű�� �ڷ�ƾ
    IEnumerator MoveCameraToTarget()
    {
        isMoving = true;

        Vector3 startPosition = Camera.main.transform.position;
        Quaternion startRotation = Camera.main.transform.rotation;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            // �ð��� ���� ��ġ�� ȸ���� �ε巴�� ����
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / duration);
            Camera.main.transform.rotation = Quaternion.Lerp(startRotation, targetPosition.rotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;  // ���� �����ӱ��� ���
        }

        // ������ ��ġ�� ȸ���� ��ǥ ������ ��Ȯ�ϰ� ����
        Camera.main.transform.position = targetPosition.position;
        Camera.main.transform.rotation = targetPosition.rotation;

        isMoving = false;
    }
}