using UnityEngine;

public class SkillCleanGuardian : ActiveSkill
{
    public float angularSpeed = 90f;
    public float angle;

    private Transform target;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        angle += angularSpeed * Mathf.Deg2Rad * Time.deltaTime;
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
            enemy.OnDamage(skillData.damage, transform.position, transform.forward);
        }
    }
}
