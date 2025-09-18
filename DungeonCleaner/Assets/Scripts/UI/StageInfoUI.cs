using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    public TextMeshProUGUI killCountText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Slider expSlider;

    [SerializeField]
    private GameObject warningMessage;
    [SerializeField]
    private Image bombFlash;

    public void SetTimeText(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        timeText.text = time.ToString(@"mm\:ss");
    }

    public void SetGoldText(int gold)
    {
        goldText.text = gold.ToString();
    }

    public void SetKillCountText(int count)
    {
        killCountText.text = count.ToString();
    }

    public void SetLevelText(int level)
    {
        levelText.text = $"Lv.{level}";
    }

    public void SetExpSliderValue(float minValue, float maxValue, float value)
    {
        expSlider.minValue = minValue;
        expSlider.maxValue = maxValue;
        expSlider.value = value;
    }

    public void StartWarningMessage()
    {
        StartCoroutine(CoWarning());
    }

    private IEnumerator CoWarning()
    {
        warningMessage.SetActive(true);
        yield return new WaitForSeconds(5f);
        warningMessage.SetActive(false);
    }

    public void StartBombFlashEffect()
    {
        StartCoroutine(CoBombFlash());
    }

    private IEnumerator CoBombFlash()
    {
        yield return StartCoroutine(FadeAlpha(0f, 1f, 0.125f));
        yield return StartCoroutine(FadeAlpha(1f, 0f, 0.125f));
    }

    private IEnumerator FadeAlpha(float from, float to, float time)
    {
        float t = 0f;
        Color c = bombFlash.color;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            c.a = Mathf.Lerp(from, to, t);
            bombFlash.color = c;
            yield return null;
        }
    }
}
