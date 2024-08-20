using UnityEngine;

public class GridItemPlacement : MonoBehaviour
{
    public GameObject[] itemPrefabs; // 아이템 프리팹 배열 (랜덤 선택될 아이템들)
    public int gridSize = 3;         // 그리드 크기 (3x3)
    public float spacing = 2.0f;     // 아이템 간의 간격

    private int currentIndex = 0;    // 현재 배치될 그리드의 인덱스

    void Start()
    {
        // 첫 번째 아이템 배치를 시작합니다.
        PlaceNextItem();
    }

    void PlaceNextItem()
    {
        if (currentIndex < gridSize * gridSize)
        {
            // 랜덤한 아이템 선택
            GameObject selectedItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            
            // 선택된 아이템을 인스턴스화
            GameObject newItem = Instantiate(selectedItem);

            // 그리드 위치 계산 (Y축을 따라 세로로 배치)
            int column = currentIndex / gridSize;
            int row = currentIndex % gridSize;

            // 아이템 위치 설정 (Y축을 따라 세로로 배치, X축으로 열 이동)
            Vector3 newPosition = new Vector3(column * spacing, -row * spacing, 0);
            newItem.transform.position = newPosition;

            // 다음 그리드 인덱스로 이동
            currentIndex++;

            // 일정 시간 후 다음 아이템을 배치
            Invoke("PlaceNextItem", 1.0f); // 1초 간격으로 배치
        }
    }
}