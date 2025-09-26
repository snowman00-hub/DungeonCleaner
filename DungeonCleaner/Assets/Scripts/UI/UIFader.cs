using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    public float fadeDuration = 0.3f;
    private CanvasGroup canvasGroup;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void StartFadeInOut()
    {
        if (fadeCoroutine == null)
            fadeCoroutine = StartCoroutine(FadeLoop(fadeDuration));
    }

    public void StopFadeInOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
            canvasGroup.alpha = 1.0f;
        }
    }

    private IEnumerator FadeLoop(float duration)
    {
        while (true)
        {
            // 페이드 아웃
            yield return StartCoroutine(Fade(1, 0, duration));
            // 페이드 인
            yield return StartCoroutine(Fade(0, 1, duration));
        }
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}

