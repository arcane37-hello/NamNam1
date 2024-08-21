using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabs : MonoBehaviour
{
    // 이 스크립트가 부착된 오브젝트와 트리거된 오브젝트를 파괴할 수 있도록 합니다.
    private void OnTriggerEnter(Collider other)
    {
        // 트리거된 오브젝트의 프리팹을 얻어옵니다.
        GameObject hitObject = other.gameObject;

        // 트리거된 오브젝트가 특정 태그를 가진 오브젝트인지 확인합니다.
        if (hitObject.CompareTag("Fruit"))
        {
            // 오브젝트를 파괴합니다.
            Destroy(hitObject);
        }
    }
}