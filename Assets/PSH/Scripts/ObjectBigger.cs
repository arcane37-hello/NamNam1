using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectBigger : MonoBehaviour
{
    public GameObject parent;            // 크기를 조절할 부모 오브젝트
    public float scaleIncrement = 0.1f;  // 크기를 증가시킬 값
    private int keyPressCount = 0;       // J 키를 누른 횟수를 저장하는 변수
    public Text countText;               // 텍스트 컴포넌트 (인스펙터에서 할당)

    void Start()
    {
        if (countText != null)
        {
            // 초기 텍스트를 0으로 설정
            countText.text = keyPressCount.ToString();
        }
        else
        {
            Debug.LogError("Count Text가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))  // J 키를 눌렀을 때
        {
            keyPressCount++;  // 키 입력 횟수 증가

            // 오브젝트의 크기를 증가시킴
            transform.localScale += new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

            // 텍스트를 업데이트
            if (countText != null)
            {
                countText.text = keyPressCount.ToString();
            }

            // J 키를 30번 누르면 부모 오브젝트를 비활성화
            if (keyPressCount >= 30)
            {
                if (parent != null)
                {
                    parent.SetActive(false);  // 부모 오브젝트를 비활성화
                }
                else
                {
                    Debug.LogWarning("Parent GameObject is not assigned.");
                }
            }
        }
    }
}