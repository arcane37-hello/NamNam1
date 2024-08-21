using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator animator;
    private bool open = false;
    public GameObject[] randomItems; // 박스에서 생성될 아이템들
    public Transform spawnPoint;
    public ItemManager itemManager; // ItemManager 참조
    private AudioSource audioSource;

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
    }

    void OnMouseDown() // 클릭 이벤트를 처리하는 메서드
    {
        if (!open) // 현재 문이 열려 있지 않은 상태일 때
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
    }

    void SpawnRandomItem()
    {
        // 랜덤한 아이템 생성
        int randomIndex = Random.Range(0, randomItems.Length);
        GameObject randomItem = Instantiate(randomItems[randomIndex], spawnPoint.position, spawnPoint.rotation);

        // 생성된 아이템의 이름을 가져와서 리스트에 추가
        string itemName = randomItem.name.Replace("(Clone)", "").Trim();
        if (itemManager != null)
        {
            itemManager.AddItem(itemName); // 아이템 리스트에 추가
        }
        else
        {
            Debug.LogError("ItemManager 참조가 설정되어 있지 않습니다.");
        }

        // 2초 후에 아이템 제거
        Destroy(randomItem, 2.0f);

        // 뾰로롱 소리 재생 (선택 사항)
        // if (audioSource != null && soundEffect != null)
        // {
        //     audioSource.PlayOneShot(soundEffect);
        // }
    }
}