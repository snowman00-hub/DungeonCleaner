using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    public float fadeDuration = 0.3f;
    private CanvasGroup canvasGroup;

    private Coroutine fadeCoroutine;
    private Coroutine singleFadeCoroutine;

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
        }

        if (singleFadeCoroutine != null)
        {
            StopCoroutine(singleFadeCoroutine);
            singleFadeCoroutine = null;
        }

        canvasGroup.alpha = 1.0f;
    }

    private IEnumerator FadeLoop(float duration)
    {
        while (true)
        {
            singleFadeCoroutine = StartCoroutine(Fade(1, 0, duration));
            yield return singleFadeCoroutine;

            singleFadeCoroutine = StartCoroutine(Fade(0, 1, duration));
            yield return singleFadeCoroutine;
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