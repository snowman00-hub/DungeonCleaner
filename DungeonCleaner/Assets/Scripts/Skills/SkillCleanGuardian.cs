using UnityEngine;

public class SkillCleanGuardian : ActiveSkill
{
    public float angle;

    private Transform target;

    protected override void Awake()
    {
        base.Awake();
    }

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tag.Enemy))
        {
            var enemy = other.GetComponent<Enemy>();
            int finalDamage = Mathf.FloorToInt((skillData.damage + Player.Instance.data.atk) * Player.Instance.data.finalAttackMultiplier);
            enemy.OnDamage(finalDamage, enemy.transform.position, transform.forward);
            enemy.KnockBack(1.5f);
        }
    }
}
