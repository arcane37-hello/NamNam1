using UnityEngine;
using System;
using System.Collections;

public class Box : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] randomItems; // 박스에서 생성될 아이템들
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject popCanvas; // Pop Canvas 추가

    private bool open = false;
    private ItemDataService itemDataService;
    public AudioSource PungAudio;

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (animator == null) animator = GetComponent<Animator>();
   

        itemDataService = FindObjectOfType<ItemDataService>();
        if (itemDataService == null)
        {
            Debug.LogError("ItemDataService를 찾을 수 없습니다. ItemDataService를 씬에 추가하세요.");
        }

        if (popCanvas != null)
        {
            popCanvas.SetActive(false); // 처음엔 Pop Canvas를 비활성화
        }
        else
        {
            Debug.LogWarning("Pop Canvas가 설정되어 있지 않습니다.");
        }
    }

    void OnMouseDown()
    {
        if (!open)
        {
            OpenBox();
        }
    }

    void OpenBox()
    {
        open = true;
        animator.SetBool("open", open);
        ShowPopCanvas(); // 박스 열리면서 Pop Canvas 표시
        PungAudio.Play();
        StartCoroutine(SpawnItemAfterDelay());
    }
    

    void ShowPopCanvas()
    {
        if (popCanvas != null)
        {
            StartCoroutine(ShowAndHidePopCanvasWithDelay());
        }
    }

    IEnumerator ShowAndHidePopCanvasWithDelay()
    {
        yield return new WaitForSeconds(1f); // 0.5초 딜레이 추가
       
        popCanvas.SetActive(true);
        StartCoroutine(HidePopCanvasAfterDelay());
        
    }

    
    IEnumerator HidePopCanvasAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // 1초 후에 Pop Canvas를 비활성화
        popCanvas.SetActive(false);
    }

    IEnumerator SpawnItemAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // 애니메이션 시간에 맞춰 딜레이 설정
        SpawnRandomItem();
    }

    void SpawnRandomItem()
    {
        if (randomItems.Length == 0)
        {
            Debug.LogError("No random items assigned to the box.");
            return;
        }

        Vector3 spawnPosition;
        if (spawnPoint != null)
        {
            spawnPosition = spawnPoint.position;
            Debug.Log($"Using spawnPoint position: {spawnPosition}");
        }
        else
        {
            spawnPosition = transform.position;
            Debug.LogWarning($"spawnPoint is null. Using Box position: {spawnPosition}");
        }

        int randomIndex = UnityEngine.Random.Range(0, randomItems.Length);
        GameObject randomItem = Instantiate(randomItems[randomIndex], spawnPosition, Quaternion.identity);
        Debug.Log($"Spawned item: {randomItem.name} at position: {randomItem.transform.position}");

        ItemBehavior item = randomItem.GetComponent<ItemBehavior>();
        if (item != null)
        {
            InitializeItemData(item);
            SaveItemToServer(item);
        }
        else
        {
            Debug.LogError("Spawned item does not have ItemBehavior component.");
        }

        StartCoroutine(DestroyItemAfterDelay(randomItem));
    }

    void InitializeItemData(ItemBehavior item)
    {
        item.uuid = UUIDManager.GetUUID();  // 항상 사용자의 UUID 사용
        item.date = DateTime.Now.ToString("yyyy-MM-dd");
        item.special = UnityEngine.Random.value > 0.8f; // 20% 확률로 특별 아이템

        // 주의: itemName은 프리팹에 이미 설정되어 있다고 가정합니다.
    }

    void SaveItemToServer(ItemBehavior item)
    {
        if (itemDataService != null)
        {
            StartCoroutine(itemDataService.SaveItemData(item));
        }
        else
        {
            Debug.LogError("ItemDataService 참조가 설정되어 있지 않습니다.");
        }
    }

    IEnumerator DestroyItemAfterDelay(GameObject item)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(item);
        Destroy(gameObject);
    }
}