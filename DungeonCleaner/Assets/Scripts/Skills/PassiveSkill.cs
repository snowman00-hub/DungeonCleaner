using UnityEngine;

public enum StatType
{
    Attack = 1,
    Defense = 2,
    MaxHp = 3,
    Speed = 4,
    ActiveSkillDuration = 5,
}

public enum PassiveSkillName
{
    atkIncrease,
}

public class PassiveSkill : MonoBehaviour
{
    public PassiveSkillName passiveSkillName;

    [HideInInspector]
    public int level = 1;
    [HideInInspector]
    public PassiveSkillData data;

    private void Awake()
    {
        data = DataTableManger.PassiveSkillTable.Get($"{passiveSkillName.ToString()}{level}");
    }
}
