using System.Collections;
using UnityEngine;

public class SkillDustGun : ActiveSkill
{
    [HideInInspector]
    public float waitingTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetDirection(7f);
    }

    private void Update()
    {
        transform.position += dir * skillData.projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            var enemy = other.GetComponent<Enemy>();
            int finalDamage = Mathf.FloorToInt((skillData.damage + Player.Instance.data.atk) * Player.Instance.data.finalAttackMultiplier);
            enemy.OnDamage(finalDamage, enemy.transform.position, enemy.transform.forward);
            OnUsed?.Invoke();
        }
    }
}
