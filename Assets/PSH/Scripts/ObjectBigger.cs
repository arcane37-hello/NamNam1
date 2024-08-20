using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBigger : MonoBehaviour
{
    public float scaleIncrement = 0.1f;  // 크기를 증가시킬 값

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))  // J 키를 눌렀을 때
        {
            // 오브젝트의 크기를 증가시킴
            transform.localScale += new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

            // 현재 크기가 4를 초과하면 오브젝트 파괴
            if (transform.localScale.x > 4f || transform.localScale.y > 4f || transform.localScale.z > 4f)
            {
                Destroy(gameObject);
            }
        }
    }
}