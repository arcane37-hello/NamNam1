using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartRefrigerator : MonoBehaviour
{
    public Image fadeInImage;             // ���̵� ���� �̹��� (�ν����Ϳ��� �Ҵ�)
    public float fadeDuration = 1.0f;     // ���̵� ���ϴ� �� �ɸ��� �ð�
    private void Start()
    {
        if (fadeInImage == null)
        {
            Debug.LogError("���̵� ���� �̹����� �Ҵ���� �ʾҽ��ϴ�.");
        }
        else
        {
            // �̹����� ���� ���� 1���� �����Ͽ� ó������ ���̰�
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
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // ���� ���� ��

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, (elapsedTime / fadeDuration));
            fadeInImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ���� �� ����
        fadeInImage.color = endColor;
    }
}
