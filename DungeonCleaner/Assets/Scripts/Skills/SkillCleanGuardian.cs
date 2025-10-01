using UnityEngine;

public class SkillCleanGuardian : ActiveSkill
{
    public float angle;
    public GameObject defaultAsset;
    public GameObject awakeningAsset;

    private Transform target;

    private void Update()
    {
        angle += skillData.projectileSpeed * Mathf.Deg2Rad * Time.deltaTime;
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * skillData.radius;
        transform.position = target.position + offset;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        target = Player.Instance.transform;

        if (skillData.skillLevel == 6)
        {
            defaultAsset.SetActive(false);
            awakeningAsset.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            var enemy = other.GetComponent<Enemy>();
            int finalDamage = Mathf.FloorToInt((skillData.damage + Player.Instance.data.atk) * Player.Instance.data.finalAttackMultiplier);
            enemy.OnDamage(finalDamage, enemy.transform.position, transform.forward);
            ActiveSkillManager.Instance.damageAmounts[skillName] += finalDamage;
            enemy.KnockBack(2.5f);
        }
    }
}
