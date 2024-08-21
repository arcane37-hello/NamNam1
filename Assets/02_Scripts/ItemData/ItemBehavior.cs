using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public string uuid;
    public string itemName;
    public string date;
    public bool special;

    private Renderer itemRenderer;

    private void Awake()
    {
        itemRenderer = GetComponent<Renderer>();
        if (itemRenderer != null)
        {
            //SetVisibility(0.5f); // false로 설정하여 기본적으로 반투명 설정
        }
        else
        {
            Debug.LogError($"{gameObject.name} does not have a Renderer component.");
        }
    }

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
            //SetColor(Color.gold);
        }
        else
        {
            // 일반 아이템 시각화 로직
            SetColor(Color.white);
        }
        gameObject.name = $"{itemName} ({date})";
    }

    public void SetVisibility(float isVisible)
    {
        if (itemRenderer != null)
        {
            Color color = itemRenderer.material.color;
            color.a = isVisible;
            itemRenderer.material.color = color;
        }
        else
        {
            Debug.LogError($"{gameObject.name} does not have a Renderer component.");
        }
    }

    private void SetTransparency(float alpha)
    {
        if (itemRenderer != null)
        {
            Color color = itemRenderer.material.color;
            color.a = alpha;
            itemRenderer.material.color = color;
        }
    }

    private void SetColor(Color newColor)
    {
        if (itemRenderer != null)
        {
            newColor.a = itemRenderer.material.color.a; // 현재 투명도 유지
            itemRenderer.material.color = newColor;
        }
    }
}