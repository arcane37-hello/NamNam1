using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class ItemDataService : MonoBehaviour
{
    private const string SAVE_ITEM_URL = "http://172.16.17.131:8080/api/v1/users/refrigerator";

    [Serializable]
    private class ItemData
    {
        public string uuid;      // ������� UUID
        public string itemName;
        public string date;
        public bool special;
    }

    public IEnumerator SaveItemData(ItemBehavior item)
    {
        ItemData itemData = new ItemData
        {
            uuid = item.uuid,
            itemName = item.itemName,
            date = item.date,
            special = item.special
        };

        string json = JsonUtility.ToJson(itemData);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(SAVE_ITEM_URL, json))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Item data saved to server successfully.");
            }
            else
            {
                Debug.LogError($"Error saving item data to server: {www.error}");
            }
        }
    }
}