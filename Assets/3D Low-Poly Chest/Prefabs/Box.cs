using UnityEngine;
using System;
using System.Collections;

public class Box : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private GameObject[] randomItems; // 박스에서 생성될 아이템들
    [SerializeField] private Transform spawnPoint;

    private bool isOpen = false;
    private ItemManager itemManager;
    private ItemDataService itemDataService;

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        itemManager = FindObjectOfType<ItemManager>();
        if (itemManager == null)
        {
            Debug.LogError("ItemManager를 찾을 수 없습니다. ItemManager를 씬에 추가하거나 연결하세요.");
        }

        itemDataService = FindObjectOfType<ItemDataService>();
        if (itemDataService == null)
        {
            Debug.LogError("ItemDataService를 찾을 수 없습니다. ItemDataService를 씬에 추가하세요.");
        }
    }

    void OnMouseDown()
    {
        if (!isOpen)
        {
            OpenBox();
        }
    }

    void OpenBox()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        PlayOpenSound();
        StartCoroutine(SpawnItemAfterDelay());
    }

    void PlayOpenSound()
    {
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
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

        int randomIndex = UnityEngine.Random.Range(0, randomItems.Length);
        GameObject randomItem = Instantiate(randomItems[randomIndex], spawnPoint.position, spawnPoint.rotation);

        ItemBehavior item = randomItem.GetComponent<ItemBehavior>();
        if (item != null)
        {
            InitializeItemData(item);
            AddItemToManager(item);
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
        item.Initialize(
            Guid.NewGuid().ToString(),
            item.name,
            DateTime.Now.ToString("yyyy-MM-dd"),
            UnityEngine.Random.value > 0.8f // 20% 확률로 특별 아이템
        );
    }

    void AddItemToManager(ItemBehavior item)
    {
        if (itemManager != null)
        {
            itemManager.AddItem(item);
        }
        else
        {
            Debug.LogError("ItemManager 참조가 설정되어 있지 않습니다.");
        }
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
        yield return new WaitForSeconds(2.0f);
        Destroy(item);
    }
}