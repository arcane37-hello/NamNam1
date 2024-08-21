using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class RefrigeratorManager : MonoBehaviour
{
    [SerializeField] private Button refrigeratorButton;
    [SerializeField] private TMP_Dropdown yearDropdown;
    [SerializeField] private TMP_Dropdown monthDropdown;
    [SerializeField] private ItemBehavior[] itemBehaviors;

    private const string MYREFRIGERATOR_URL = "http://172.16.17.131:8080/api/v1/users/refrigerator/my";

    [System.Serializable]
    public class RefrigeratorRequest
    {
        public string uuid;
        public string date;
    }

    [System.Serializable]
    public class RefrigeratorItem
    {
        public string uuid;
        public string itemName;
        public string date;
        public bool special;
    }

    [System.Serializable]
    private class RefrigeratorResponse
    {
        public RefrigeratorItem[] items;
    }

    private void Start()
    {
        if (refrigeratorButton != null)
        {
            refrigeratorButton.onClick.AddListener(OnRefrigeratorButtonClick);
        }
        OnRefrigeratorButtonClick();
        InitializeDateDropdowns();
        //SetAllItemsSemiTransparent();
    }

    private void InitializeDateDropdowns()
    {
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;

        // 현재 년도에 해당하는 옵션 찾기
        int yearIndex = yearDropdown.options.FindIndex(option => option.text == currentYear.ToString());
        if (yearIndex != -1)
        {
            yearDropdown.value = yearIndex;
        }
        else
        {
            Debug.LogWarning("Current year not found in dropdown options. Defaulting to first option.");
            yearDropdown.value = 0;
        }

        // 현재 월 선택 (1-based to 0-based index)
        if (currentMonth >= 1 && currentMonth <= monthDropdown.options.Count)
        {
            monthDropdown.value = currentMonth - 1;
        }
        else
        {
            Debug.LogWarning("Current month out of range. Defaulting to first option.");
            monthDropdown.value = 0;
        }

        yearDropdown.RefreshShownValue();
        monthDropdown.RefreshShownValue();
    }

    private void SetAllItemsSemiTransparent()
    {
        foreach (ItemBehavior item in itemBehaviors)
        {
            if (item != null)
            {
                item.SetVisibility(0.5f);  // 0.5f로 설정하여 반투명 상태로 만듦
                Debug.Log($"Set {item.itemName} to semi-transparent");
            }
        }
    }

    private void OnRefrigeratorButtonClick()
    {
        string uuid = UUIDManager.GetUUID();
        int selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int selectedMonth = monthDropdown.value + 1;
        int lastDayOfMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
        string selectedDate = $"{selectedYear}-{selectedMonth:00}-{lastDayOfMonth:00}";

        Debug.Log($"Sending request for date: {selectedDate}");
        StartCoroutine(SendRefrigeratorRequest(uuid, selectedDate));
    }

    private IEnumerator SendRefrigeratorRequest(string uuid, string date)
    {
        RefrigeratorRequest refrigeratorRequest = new RefrigeratorRequest
        {
            uuid = uuid,
            date = date
        };
        string jsonBody = JsonConvert.SerializeObject(refrigeratorRequest);
        using (UnityWebRequest www = new UnityWebRequest(MYREFRIGERATOR_URL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log($"Received response: {jsonResponse}");

                //RefrigeratorResponse response = JsonConvert.DeserializeObject<RefrigeratorResponse>(jsonResponse);
                RefrigeratorItem[] responseBody = JsonConvert.DeserializeObject<RefrigeratorItem[]>(jsonResponse);
                RefrigeratorResponse response = new RefrigeratorResponse();
                response.items = responseBody;
                if (response != null && response.items != null)
                {
                    SyncItemsToGameObjects(response.items);
                }
                else
                {
                    Debug.LogError("Failed to parse response or items list is null");
                }
            }
            else
            {
                Debug.LogError($"Failed to fetch refrigerator data: {www.error}");
            }
        }
    }

    private void SyncItemsToGameObjects(RefrigeratorItem[] items)
    {
        Debug.Log($"Syncing {items.Length} items");

        // 먼저 모든 아이템을 반투명하게 설정
        SetAllItemsSemiTransparent();
        print("1");
        // 서버에서 받은 아이템 정보에 해당하는 프리팹만 완전히 불투명하게 설정
        foreach (RefrigeratorItem item in items)
        {
            
            ItemBehavior matchingItem = Array.Find(itemBehaviors, behavior => behavior.itemName.Equals(item.itemName, StringComparison.OrdinalIgnoreCase));
            if (matchingItem != null)
            {
                matchingItem.Initialize(item.uuid, item.itemName, item.date, item.special);
                matchingItem.SetVisibility(1f);  // 1f로 설정하여 완전히 불투명하게 만듦
                Debug.Log($"Set {item.itemName} to fully visible");
            }
            else
            {
                Debug.LogWarning($"Item not found for: {item.itemName}");
            }
        }

        Debug.Log($"Synchronized {items.Length} items from the server.");
    }
}