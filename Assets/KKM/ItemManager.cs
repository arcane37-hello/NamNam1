using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private List<ItemBehavior> items = new List<ItemBehavior>();

    public void AddItem(ItemBehavior item)
    {
        items.Add(item);
        Debug.Log($"Added item: {item.itemName} to the list. Total items: {items.Count}");
    }
}