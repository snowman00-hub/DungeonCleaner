using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public int maxHP = 100;

    public int HP { get; private set; }
    public bool IsDead { get; private set; }

    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        IsDead = false;
        HP = maxHP;
    }

    public virtual void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        HP -= damage;

        if (HP <= 0 && !IsDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;
    }
}