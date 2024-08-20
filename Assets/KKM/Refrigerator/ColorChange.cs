using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour
{
    public GameObject targetCube;       // 색깔이 변할 대상 큐브
    public Button toggleColorButton;    // 색깔을 바꿀 버튼
    public Color grayColor = Color.gray; // 회색 (고정된 색)

    private Renderer cubeRenderer;
    private Color originalColor;         // 큐브의 원래 색깔
    private bool isGray = false;         // 현재 큐브가 회색인지 여부

    void Start()
    {
        // 큐브의 Renderer를 가져옵니다.
        cubeRenderer = targetCube.GetComponent<Renderer>();

        // 큐브의 원래 색깔을 저장합니다.
        originalColor = cubeRenderer.material.color;

        // 버튼 클릭 이벤트에 메서드를 연결합니다.
        toggleColorButton.onClick.AddListener(ToggleColor);
    }

    void ToggleColor()
    {
        if (isGray)
        {
            // 현재 회색이면 원래 색깔로 돌아갑니다.
            cubeRenderer.material.color = originalColor;
        }
        else
        {
            // 현재 원래 색깔이면 회색으로 변합니다.
            cubeRenderer.material.color = grayColor;
        }

        // 상태를 반전시킵니다.
        isGray = !isGray;
    }
}