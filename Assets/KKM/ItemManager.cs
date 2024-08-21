using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<string> collectedItems = new List<string>(); // 획득한 아이템 리스트

    // 아이템을 리스트에 등록하는 메서드
    public void AddItem(string itemName)
    {
        collectedItems.Add(itemName);
        Debug.Log(itemName + " has been added to the inventory.");
    }
}
