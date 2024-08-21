using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RefrigeratorManager : MonoBehaviour
{
    [SerializeField] private Button refrigeratorButton;
    [SerializeField] private TMP_Dropdown yearDropdown;
    [SerializeField] private TMP_Dropdown monthDropdown;

    private const string MYREFRIGERATOR_URL = "http://172.16.17.131:8080/api/v1/users/refrigerator/my";

    [Serializable]
    private class RefrigeratorRequest
    {
        public string uuid;
        public string date;
    }

    [Serializable]
    private class RefrigeratorItem
    {
        public string uuid;
        public string itemName;
        public string date;
        public bool special;
    }

    private void Start()
    {
        if (refrigeratorButton != null)
        {
            refrigeratorButton.onClick.AddListener(OnRefrigeratorButtonClick);
        }
        InitializeDateDropdowns();
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

    private void OnRefrigeratorButtonClick()
    {
        string uuid = UUIDManager.GetUUID();
        int selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int selectedMonth = monthDropdown.value + 1;
        int lastDayOfMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
        string selectedDate = $"{selectedYear}-{selectedMonth:00}-{lastDayOfMonth:00}";

        StartCoroutine(SendRefrigeratorRequest(uuid, selectedDate));
    }

    private IEnumerator SendRefrigeratorRequest(string uuid, string date)
    {
        RefrigeratorRequest refrigeratorRequest = new RefrigeratorRequest
        {
            uuid = uuid,
            date = date
        };
        string jsonBody = JsonUtility.ToJson(refrigeratorRequest);
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
                List<RefrigeratorItem> items = JsonUtility.FromJson<List<RefrigeratorItem>>("{\"items\":" + jsonResponse + "}");

                SyncItemsToGameObjects(items);
            }
            else
            {
                Debug.LogError($"Failed to fetch refrigerator data: {www.error}");
            }
        }
    }

    private void SyncItemsToGameObjects(List<RefrigeratorItem> items)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (RefrigeratorItem item in items)
        {
            GameObject itemObject = new GameObject($"Item_{item.itemName}");
            itemObject.transform.SetParent(transform);
            ItemBehavior itemBehavior = itemObject.AddComponent<ItemBehavior>();
            itemBehavior.Initialize(item.uuid, item.itemName, item.date, item.special);
        }

        Debug.Log($"Synchronized {items.Count} items from the server.");
    }
}