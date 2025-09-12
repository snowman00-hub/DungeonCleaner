using System;
using UnityEngine;
using UnityEngine.Audio;

public enum EnemyName
{
    Mushroom,
}

public class EnemyHealth : LivingEntity
{
    public static readonly int hashHurt = Animator.StringToHash("Hurt");

    public EnemyName enemyName;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);
        animator.SetTrigger(hashHurt);
    }

    protected override void Die()
    {
        base.Die();
    }
}
