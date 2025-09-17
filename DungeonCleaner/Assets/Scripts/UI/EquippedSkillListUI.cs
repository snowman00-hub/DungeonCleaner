using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSkillListUI : MonoBehaviour
{
    public List<Image> activeSkillImages;
    public List<Image> passiveSkillImages;

    private void OnEnable()
    {
        for (int i = 0; i < ActiveSkillManager.Instance.equippedSkills.Count; i++)
        {
            activeSkillImages[i].sprite = ActiveSkillManager.Instance.equippedSkills[i].skillSprite;
            activeSkillImages[i].enabled = true;
        }

        for (int i = 0; i < PassiveSkillManager.Instance.equippedSkills.Count; i++)
        {
            passiveSkillImages[i].sprite = PassiveSkillManager.Instance.equippedSkills[i].sprite;
            passiveSkillImages[i].enabled = true;
        }
    }
}
