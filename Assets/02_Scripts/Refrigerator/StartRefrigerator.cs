using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartRefrigerator : MonoBehaviour
{
    public Image fadeInImage;             // 페이드 인할 이미지 (인스펙터에서 할당)
    public float fadeDuration = 1.0f;     // 페이드 인하는 데 걸리는 시간
    private void Start()
    {
        if (fadeInImage == null)
        {
            Debug.LogError("페이드 인할 이미지가 할당되지 않았습니다.");
        }
        else
        {
            // 이미지의 알파 값을 1으로 설정하여 처음에는 보이게
            Color color = fadeInImage.color;
            color.a = 1;
            fadeInImage.color = color;
        }
        if (fadeInImage != null)
        {
            StartCoroutine(FadeInImage());
        }
    }

    IEnumerator FadeInImage()
    {
        float elapsedTime = 0.0f;
        Color startColor = fadeInImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // 최종 알파 값

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, (elapsedTime / fadeDuration));
            fadeInImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 알파 값 설정
        fadeInImage.color = endColor;
    }
}
