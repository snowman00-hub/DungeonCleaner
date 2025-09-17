using System.Collections.Generic;
using UnityEngine;

public enum PassiveSkillName
{
    atkIncrease,
    defIncrease,
    hpIncrease,
    msIncrease,
    skilltimeIncrease,
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
        foreach (var passiveSkill in allSkillList)
        {
            passiveSkill.data = DataTableManger.PassiveSkillTable.Get($"{passiveSkill.passiveSkillName}{1}");
        }
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
                player.data.finalAttackMultiplier += skill.data.PASSIVE_VALUE / 100;
                break;
            case StatType.Defense:
                player.data.finalDamageReduction += skill.data.PASSIVE_VALUE / 100;
                break;
            case StatType.MaxHp:
                player.MaxHpUp((int)(player.data.InitialMaxHP * (skill.data.PASSIVE_VALUE / 100)));
                break;
            case StatType.Speed:
                player.data.speed += player.data.InitialSpeed * (skill.data.PASSIVE_VALUE / 100);
                break;
            case StatType.ActiveSkillDuration:
                player.data.activeSkillDurationMultiplier += skill.data.PASSIVE_VALUE / 100;
                break;
        }
    }
}