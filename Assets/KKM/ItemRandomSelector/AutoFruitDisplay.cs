using UnityEngine;

public class AutoFruitDisplay : MonoBehaviour
{
    public GameObject[] fruitObjects;  // 과일 오브젝트 배열
    public float displayInterval = 2f; // 과일을 교체하는 간격 (초 단위)

    private int currentIndex = 0;      // 현재 보이는 과일의 인덱스
    private float timer;

    void Start()
    {
        // 처음에는 모든 과일 오브젝트를 비활성화합니다.
        foreach (GameObject fruit in fruitObjects)
        {
            fruit.SetActive(false);
        }

        // 첫 번째 과일 오브젝트를 활성화합니다.
        if (fruitObjects.Length > 0)
        {
            fruitObjects[currentIndex].SetActive(true);
        }
    }

    void Update()
    {
        // 타이머를 업데이트합니다.
        timer += Time.deltaTime;

        // 지정된 간격이 지났다면 다음 과일로 전환합니다.
        if (timer >= displayInterval && currentIndex < fruitObjects.Length - 1)
        {
            // 다음 과일 인덱스로 이동합니다.
            currentIndex++;

            // 다음 과일 오브젝트를 활성화합니다.
            fruitObjects[currentIndex].SetActive(true);

            // 타이머를 리셋합니다.
            timer = 0f;
        }
    }
}