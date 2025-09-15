using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private TextMeshProUGUI killCountText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Slider expSlider;

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
}
