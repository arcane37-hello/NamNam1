using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public string uuid;
    public string itemName;
    public string date;
    public bool special;

    public void Initialize(string uuid, string itemName, string date, bool special)
    {
        this.uuid = uuid;
        this.itemName = itemName;
        this.date = date;
        this.special = special;

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (special)
        {
            // 특별 아이템 시각화 로직
            // 예: GetComponent<Renderer>().material.color = Color.gold;
        }
        else
        {
            // 일반 아이템 시각화 로직
            // 예: GetComponent<Renderer>().material.color = Color.white;
        }

        gameObject.name = $"{itemName} ({date})";
    }
}
