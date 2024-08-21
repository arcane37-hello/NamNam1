using UnityEngine;

public class ItemColorManager : MonoBehaviour
{
    // 아이템의 원래 Material과 획득 여부를 저장하는 클래스
    [System.Serializable]
    public class Item
    {
        public GameObject itemObject;     // 아이템의 게임 오브젝트
        public Material originalMaterial; // 아이템의 원래 Material
        public bool isCollected = false;  // 아이템의 획득 여부

        // 생성자에서 초기 회색 Material 설정
        public void Initialize(GameObject obj, Material original, Material gray)
        {
            itemObject = obj;
            originalMaterial = original;

            // 초기 상태로 회색 Material을 적용
            if (itemObject != null)
            {
                itemObject.GetComponent<Renderer>().material = gray;
            }
        }

        // 아이템을 획득할 때 원래 Material로 복구
        public void Collect()
        {
            if (!isCollected && itemObject != null)
            {
                isCollected = true;
                // 아이템의 Material을 원래 Material로 변경
                itemObject.GetComponent<Renderer>().material = originalMaterial;
            }
        }
    }

    public Material grayMaterial;  // 공통 회색 Material
    public Item[] items;

    void Start()
    {
        // 각 아이템 초기화 (GameObject와 originalMaterial이 설정된 경우)
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Initialize(items[i].itemObject, items[i].originalMaterial, grayMaterial);
        }
    }

    // 특정 아이템을 획득할 때 호출되는 함수
    public void CollectItem(int index)
    {
        if (index >= 0 && index < items.Length)
        {
            items[index].Collect();
        }
    }

    // 테스트를 위한 업데이트 함수
    void Update()
    {
        // 1번 아이템을 스페이스바로 획득하는 테스트 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CollectItem(0);  // 예: 첫 번째 아이템을 획득
        }
    }
}