using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillLevel;
    public int damage;
    public float radius;
    public float coolDown;
    public float duration;
    public float tickInterval;
    public int projectileCount;
    public float projectileSpeed;
}
