using System.Collections;
using UnityEngine;
using TMPro;

public class TMP_Toggle : MonoBehaviour
{
    public TextMeshProUGUI targetTMP; // 제어할 TextMeshPro 객체
    public float activationInterval = 5f; // TMP가 활성화될 간격 (초)
    public float displayDuration = 2f; // TMP가 표시될 시간 (초)

    private void Start()
    {
        
        // 처음 시작할 때 TMP 객체를 비활성화
        targetTMP.gameObject.SetActive(false);
        StartCoroutine(ToggleTMPRoutine());
    }

    private IEnumerator ToggleTMPRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(activationInterval); // 설정한 간격만큼 대기

            targetTMP.gameObject.SetActive(true); // TMP 활성화
            yield return new WaitForSeconds(displayDuration); // 설정한 시간만큼 대기

            targetTMP.gameObject.SetActive(false); // TMP 비활성화
        }
    }
}