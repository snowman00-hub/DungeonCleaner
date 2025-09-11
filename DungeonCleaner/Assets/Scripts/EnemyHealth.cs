using System;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyHealth : LivingEntity
{
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
