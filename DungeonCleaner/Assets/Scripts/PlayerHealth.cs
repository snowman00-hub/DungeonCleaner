using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;
    public event Action OnHurt;

    private float lastHurtTime;
    private float hurtTick = 0.3f;

    private void LateUpdate()
    {
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        healthSlider.maxValue = maxHP;
        healthSlider.value = HP;
        healthSlider.transform.position = transform.position + Vector3.back;
        healthSlider.transform.rotation = Quaternion.LookRotation(healthSlider.transform.position - Camera.main.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.SetTriggerAttack();

            if (lastHurtTime + hurtTick > Time.time)
                return;

            lastHurtTime = Time.time;
            OnDamage(enemy.damage, other.ClosestPoint(transform.position), (other.transform.position - transform.position).normalized);
        }

        if (other.CompareTag(Tag.EnemyAttack))
        {
            var projectile = other.GetComponent<EnemyProjectile>();
            projectile.OnUsed?.Invoke();
            OnDamage(projectile.damage, other.ClosestPoint(transform.position), (other.transform.position - transform.position).normalized);
        }
    }

    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);
        OnHurt?.Invoke();
    }

    protected override void Die()
    {
        base.Die();
    }
}
