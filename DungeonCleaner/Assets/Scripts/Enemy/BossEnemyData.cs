using UnityEngine;

[CreateAssetMenu(fileName = "BossEnemyData", menuName = "Scriptable Objects/BossEnemyData")]
public class BossEnemyData : EnemyData
{
    public int projectile_count;
    public PickUpType? dropItem1;
    public float? dropItemValue1;
    public PickUpType? dropItem2;
    public float? dropItemValue2;
    public PickUpType? dropItem3;
    public float? dropItemValue3;
    public BossType bossType;
}
