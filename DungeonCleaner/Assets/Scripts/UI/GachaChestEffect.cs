using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GachaChestEffect : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private Sprite closeSprite;
    [SerializeField]
    private Sprite openSprite;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void StartEffect()
    {
        StartCoroutine(CoEffect());
    }

    public float duration = 0.7f;    // 흔들리는 시간
    public float strength = 25f;     // 흔들리는 강도(픽셀 단위)

    private IEnumerator CoEffect()
    {
        RectTransform rect = image.rectTransform;
        Vector3 originalPos = rect.anchoredPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            rect.anchoredPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
        image.sprite = openSprite;

        // 테스트
        yield return new WaitForSeconds(1f);
        image.sprite = closeSprite;
    }
}