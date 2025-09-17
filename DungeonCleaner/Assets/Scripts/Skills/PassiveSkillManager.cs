using System.Collections.Generic;
using UnityEngine;

public enum PassiveSkillName
{
    atkIncrease,
}

public class PassiveSkillManager : MonoBehaviour
{
    public static PassiveSkillManager Instance;

    public List<PassiveSkill> allSkillList;
    [HideInInspector]
    public List<PassiveSkill> equippedSkills = new List<PassiveSkill>();

    private Player player;

    private void Awake()
    {
        Instance = this;
        player = GetComponent<Player>();
    }

    public void EquipSkill(PassiveSkill skill, int skillLevel)
    {
        if (skillLevel == 1)
        {
            equippedSkills.Add(skill);
        }
        else
        {
            var skillTable = DataTableManger.PassiveSkillTable;
            var levelData = skillTable.Get(skill.GetSkillLevelId(skillLevel));
            skill.data = levelData;
        }

        switch (skill.data.AFFECT_ABILITY)
        {
            case StatType.Attack:
                player.data.atk += Mathf.FloorToInt(skill.data.PASSIVE_VALUE);
                break;
            case StatType.Defense:
                player.data.def += Mathf.FloorToInt(skill.data.PASSIVE_VALUE);
                break;
            case StatType.MaxHp:
                player.MaxHpUp(Mathf.FloorToInt(skill.data.PASSIVE_VALUE));
                break;
            case StatType.Speed:
                player.data.speed += skill.data.PASSIVE_VALUE;
                break;
            case StatType.ActiveSkillDuration:
                player.data.pickUpRadius += skill.data.PASSIVE_VALUE;
                break;
        }
    }
}