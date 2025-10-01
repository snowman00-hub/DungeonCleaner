using System.Collections;
using UnityEngine;

public class SkillDustGun : ActiveSkill
{
    public GameObject defaultAsset;
    public GameObject awakeningAsset;

    [HideInInspector]
    public float waitingTime;

    protected override void OnEnable()
    {
        if (skillData.skillLevel == 6)
        {
            defaultAsset.SetActive(false);
            awakeningAsset.SetActive(true);
        }

        base.OnEnable();
        SetDirection(30f);
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
            ActiveSkillManager.Instance.damageAmounts[skillName] += finalDamage;
            OnUsed?.Invoke();
        }
    }
}
