using System.Collections.Generic;
using UnityEngine;

public enum SkillInventoryType
{
    activeInven,
    passiveInven,
}

public class SkillInventory : MonoBehaviour
{
    public List<GameObject> slots;

    public SkillInventoryType type;

    private void OnEnable()
    {
        if(type == SkillInventoryType.activeInven)
        {
            for(int i= 0;i<ActiveSkillManager.Instance.equippedSkills.Count;i++)
            {
                slots[i].SetActive(true);
                var slot = slots[i].GetComponentInParent<SkillInvenSlot>();
                slot.ShowActiveSkill(ActiveSkillManager.Instance.equippedSkills[i]);
            }
        }
        
        if(type== SkillInventoryType.passiveInven)
        {
            for(int i = 0; i < PassiveSkillManager.Instance.equippedSkills.Count; i++)
            {
                slots[i].SetActive(true);
                var slot = slots[i].GetComponentInParent<SkillInvenSlot>();
                slot.ShowPassiveSkill(PassiveSkillManager.Instance.equippedSkills[i]);
            }
        }
    }
}
