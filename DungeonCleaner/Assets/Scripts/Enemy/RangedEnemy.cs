using UnityEngine;

public class RangedEnemy : Enemy
{
    public EnemyProjectileName projectilename;
    private float lastRangedAttackTime;

    protected override void Update()
    {
        base.Update();

        if(Vector3.Distance(target.position, transform.position) < projectileRange
            && lastRangedAttackTime + projectileCoolDown < Time.time)
        {
            lastRangedAttackTime = Time.time;
            EnemyProjectileManager.Instance.Fire(projectilename, transform.position, target, damage, projectileSpeed);
        }
    }
}
