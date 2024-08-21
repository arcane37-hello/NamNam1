using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator animator;
    private bool open = false;
    public GameObject[] randomItems; // 박스에서 생성될 아이템들
    public Transform spawnPoint;
    public ItemManager itemManager; // ItemManager 참조
    private AudioSource audioSource;

    public Camera mainCamera; // 메인 카메라 참조
    public float cameraMoveSpeed = 2.0f; // 카메라가 이동하는 속도
    public float zOffset = 0.5f; // 카메라가 이동할 Z축 오프셋 값

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (itemManager == null)
        {
            itemManager = FindObjectOfType<ItemManager>(); // ItemManager를 코드로 찾기
            if (itemManager == null)
            {
                Debug.LogError("ItemManager를 찾을 수 없습니다. ItemManager를 씬에 추가하거나 연결하세요.");
            }
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 메인 카메라를 자동으로 찾기
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera를 찾을 수 없습니다. 카메라를 씬에 추가하거나 연결하세요.");
            }
        }
    }

    void OnMouseDown() // 클릭 이벤트를 처리하는 메서드
    {
        if (!open) // 현재 박스가 열려 있지 않은 상태일 때
        {
            OpenBox();
        }
    }

    void OpenBox()
    {
        open = true;
        animator.SetBool("open", open);

        // 애니메이션 후 아이템 생성
        Invoke("SpawnRandomItem", 1.0f); // 애니메이션 시간에 맞춰 딜레이 설정

        // 카메라를 Z축 방향으로 이동시키는 코루틴 시작
        StartCoroutine(MoveCamera());
    }

    void SpawnRandomItem()
    {



        // 랜덤한 아이템 생성
        int randomIndex = Random.Range(0, randomItems.Length);
        GameObject randomItem = Instantiate(randomItems[randomIndex], spawnPoint.position, spawnPoint.rotation);

        // 생성된 아이템의 이름을 가져와서 리스트에 추가
        ItemBehavior item = randomItem.GetComponent<ItemBehavior>();
        if (itemManager != null)
        {
            itemManager.AddItem(item); // 아이템 리스트에 추가
        }
        else
        {
            Debug.LogError("ItemManager 참조가 설정되어 있지 않습니다.");
        }

        // 2초 후에 아이템 제거
        Destroy(randomItem, 2.0f);
    }

    System.Collections.IEnumerator MoveCamera()
    {
        Vector3 originalPosition = mainCamera.transform.position;
        Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z + zOffset);

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * cameraMoveSpeed;
            mainCamera.transform.position = Vector3.Lerp(originalPosition, targetPosition, time);
            yield return null;
        }

        // 카메라 이동이 완료된 후 추가로 수행할 작업이 있으면 여기에 추가
    }
}