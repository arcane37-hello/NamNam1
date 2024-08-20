using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabs : MonoBehaviour
{
    // 이 스크립트가 부착된 오브젝트와 충돌한 프리팹을 파괴할 수 있도록 합니다.
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 프리팹을 얻어옵니다.
        GameObject hitObject = collision.gameObject;

        // 충돌한 오브젝트가 프리팹인지 확인하는 조건을 추가합니다.
        // 예를 들어, 특정 태그를 사용하여 확인할 수 있습니다.
        // 이 예제에서는 "PrefabTag"라는 태그를 가진 오브젝트만 파괴합니다.
        if (hitObject.CompareTag("Fruit"))
        {
            // 오브젝트를 파괴합니다.
            Destroy(hitObject);
        }
    }
}