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
            // Ư�� ������ �ð�ȭ ����
            // ��: GetComponent<Renderer>().material.color = Color.gold;
        }
        else
        {
            // �Ϲ� ������ �ð�ȭ ����
            // ��: GetComponent<Renderer>().material.color = Color.white;
        }

        gameObject.name = $"{itemName} ({date})";
    }
}
