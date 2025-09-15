using System.Collections;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public EnemyProjectileName projectilename;
    private float lastRangedAttackTime;

    private bool isMove = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        isMove = true;
    }

    protected override void Update()
    {
        if (!isMove)
            return;

        base.Update();

        if (Vector3.Distance(target.position, transform.position) < enemyData.projectileRange
            && lastRangedAttackTime + enemyData.projectileCooldown < Time.time)
        {
            lastRangedAttackTime = Time.time;
            EnemyProjectileManager.Instance.Fire(projectilename, transform.position, target, enemyData.damage, enemyData.projectileSpeed);
            animator.SetTrigger(hashAttack);
            StartCoroutine(CoWait(1f));
        }
    }

    private IEnumerator CoWait(float t)
    {
        isMove = false;
        yield return new WaitForSeconds(t);
        isMove = true;
    }
}
