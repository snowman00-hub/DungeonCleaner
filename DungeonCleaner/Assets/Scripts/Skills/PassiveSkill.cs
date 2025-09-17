using UnityEngine;

public enum StatType
{
    Attack = 1,
    Defense = 2,
    MaxHp = 3,
    Speed = 4,
    ActiveSkillDuration = 5,
}

public class PassiveSkill : MonoBehaviour
{
    public Sprite sprite;
    public PassiveSkillName passiveSkillName;

    [HideInInspector]
    public PassiveSkillData data;

    public string GetSkillLevelId(int level)
    {
        return $"{passiveSkillName}{level}";
    }
}
