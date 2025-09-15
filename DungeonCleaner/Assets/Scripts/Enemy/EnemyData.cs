using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int damage;
    public int maxHp;
    public float moveSpeed;
    public PickUpType dropExp;
    public float dropPercent;
    public float projectileRange;
    public float projectileCooldown;
    public float projectileSpeed;
}
