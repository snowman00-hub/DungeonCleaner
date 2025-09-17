using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class SkillChoiceSlotManager : MonoBehaviour
{
    public GameObject slotPrefab;

    private List<Object> selectableSkills = new List<Object>();

    private void OnEnable()
    {
        selectableSkills.Clear();
        foreach (var skill in ActiveSkillManager.Instance.allSkillList)
        {
            if (skill.skillData.skillLevel == 5)
                continue;

            selectableSkills.Add(skill);
        }

        foreach (var skill in PassiveSkillManager.Instance.allSkillList)
        {
            if (skill.data.SKILL_LEVEL == 5)
                continue;

            selectableSkills.Add(skill);
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