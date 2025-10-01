using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsWindow : MonoBehaviour
{
    public List<GameObject> lines;
    public List<Image> skillImages;
    public List<Slider> damageSliders;
    public List<TextMeshProUGUI> damageTexts;

    private void OnEnable()
    {
        var skills = ActiveSkillManager.Instance.equippedSkills;
        int sum = 0;
        List<int> damages = new List<int>();
        foreach (var dict in ActiveSkillManager.Instance.damageAmounts)
        {
            sum += dict.Value;
            damages.Add(dict.Value);
        }        

        for (int i = 0; i < skills.Count; i++)
        {
            lines[i].SetActive(true);
            if (skills[i].skillData.skillLevel < 6)
            {
                skillImages[i].sprite = skills[i].skillSprite;
            }
            else
            {
                skillImages[i].sprite = skills[i].awakeningSkillSprite;
            }

            if(sum > 0)
            {
                damageSliders[i].maxValue = sum;
                damageSliders[i].value = damages[i];
                damageTexts[i].text = $"{damages[i]} ( {Mathf.FloorToInt(damages[i] / (float)sum * 100)} % )";
            }
        }
    }
}