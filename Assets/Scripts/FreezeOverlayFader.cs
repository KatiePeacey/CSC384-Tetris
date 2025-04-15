using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FreezeOverlayFader : MonoBehaviour
{
    public static FreezeOverlayFader Instance;

    private Image overlayImage;
    private float fadeDuration = 1f;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        Instance = this;
        overlayImage = GetComponent<Image>();
        SetAlpha(0f);
    }

    public void FadeIn(float duration)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(0.4f, duration)); // 0.4 alpha = subtle
    }

    public void FadeOut(float duration)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(0f, duration));
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = overlayImage.color.a;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = overlayImage.color;
        color.a = alpha;
        overlayImage.color = color;
    }
}
