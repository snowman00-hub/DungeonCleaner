using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int maxHP;
    public int atk;
    public float finalAttackMultiplier;
    public int def;
    public float finalDamageReduction;
    public float speed;
    public float activeSkillDurationMultiplier;
    public float pickUpRadius;

    public int InitialMaxHP;
    public float InitialSpeed;
}
