using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBigger : MonoBehaviour
{
    public float scaleIncrement = 0.1f;  // 크기를 증가시킬 값
    private int keyPressCount = 0;       // J 키를 누른 횟수를 저장하는 변수

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))  // J 키를 눌렀을 때
        {
            keyPressCount++;  // 키 입력 횟수 증가

            // 오브젝트의 크기를 증가시킴
            transform.localScale += new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

            // J 키를 30번 누르면 오브젝트 파괴
            if (keyPressCount >= 30)
            {
                Destroy(gameObject);
            }
        }
    }
}