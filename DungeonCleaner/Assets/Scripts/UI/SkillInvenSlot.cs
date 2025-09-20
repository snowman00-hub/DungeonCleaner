using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInvenSlot : MonoBehaviour
{
    public Image skillIcon;
    public List<Image> starImages;

    public void ShowActiveSkill(ActiveSkill activeSkill)
    {
        skillIcon.sprite = activeSkill.skillSprite;
        for(int i= 0; i < activeSkill.skillData.skillLevel; i++)
        {
            starImages[i].enabled = true;
        }
    }

    public void ShowPassiveSkill(PassiveSkill passiveSkill)
    {
        skillIcon.sprite = passiveSkill.sprite;
        for (int i = 0; i < passiveSkill.data.SKILL_LEVEL; i++)
        {
            starImages[i].enabled = true;
        }
    }
}
