using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillChoiceSlotManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject bonusGoldPrefab;
    public GameObject bonusHealPrefab;

    private List<Object> selectableSkills = new List<Object>();

    private void OnEnable()
    {
        selectableSkills.Clear();
        foreach (var skill in ActiveSkillManager.Instance.allSkillList)
        {
            if (skill.skillData.skillLevel == 6)
                continue;

            if (skill.skillData.skillLevel == 5)
            {
                if(PassiveSkillManager.Instance.equippedSkills.Select(x => x.data.CRAFT_CODE).ToList().Contains(skill.skillData.craftCode))
                    selectableSkills.Add(skill);

                continue;
            }

            selectableSkills.Add(skill);
        }

        foreach (var skill in PassiveSkillManager.Instance.allSkillList)
        {
            if (skill.data.SKILL_LEVEL == 5)
                continue;

            selectableSkills.Add(skill);
        }

        if (selectableSkills.Count == 0)
        {
            var goldSlot = Instantiate(bonusGoldPrefab, transform).GetComponent<BonusSkill>();
            goldSlot.AddGold();
            var healSlot = Instantiate(bonusHealPrefab, transform).GetComponent<BonusSkill>();
            healSlot.Heal();
            return;
        }

        if (selectableSkills.Count <= 3)
        {
            for (int i = 0; i < selectableSkills.Count; i++)
            {
                var slot = Instantiate(slotPrefab, transform).GetComponent<SkillChoiceSlot>();
                if (selectableSkills[i] is ActiveSkill active)
                {
                    slot.IsActive = true;
                    slot.ShowActiveSkill(active);
                }
                else if (selectableSkills[i] is PassiveSkill passive)
                {
                    slot.IsActive = false;
                    slot.ShowPassiveSkill(passive);
                }
            }
        }
        else
        {
            var pickList = MyUtils.PickUnique(selectableSkills.Count, 3);
            foreach(var index in pickList)
            {
                var slot = Instantiate(slotPrefab, transform).GetComponent<SkillChoiceSlot>();
                if (selectableSkills[index] is ActiveSkill active)
                {
                    slot.IsActive = true;
                    slot.ShowActiveSkill(active);
                }
                else if (selectableSkills[index] is PassiveSkill passive)
                {
                    slot.IsActive = false;
                    slot.ShowPassiveSkill(passive);
                }
            }
        }
    }
}