using UnityEngine;

[CreateAssetMenu(fileName = "BossEnemyData", menuName = "Scriptable Objects/BossEnemyData")]
public class BossEnemyData : EnemyData
{
    public int projectile_count;
    public PickUpType dropItem;
    public float dropItemValue;
}
