using UnityEngine;

public class Enemy : LivingEntity
{
    public static readonly int hashHurt = Animator.StringToHash("Hurt");

    public float speed = 5f;
    public float avoidWeight = 1f;
    public EnemyName enemyName;
    public LayerMask enemyMask;

    private Transform target;
    private CapsuleCollider capsuleCollider;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        target = GameObject.FindWithTag(Tag.Player).transform;
    }

    private void Update()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
        // target과 y좌표는 같다고 가정
        Vector3 dir = (target.position - transform.position).normalized;

        var neighbors = Physics.OverlapSphere(transform.position, capsuleCollider.radius * 2f, enemyMask);

        Vector3 avoid = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            if (neighbor.transform == transform)
                continue;

            Vector3 avoidDir = transform.position - neighbor.transform.position;
            float distance = avoidDir.magnitude;
            // 거리가 가까울수록 미는 힘이 커짐
            if (distance > 0)
                avoid += avoidDir.normalized / distance;
        }

        Vector3 finalDir = (dir + avoid * avoidWeight).normalized;
        transform.position += finalDir * speed * Time.deltaTime;
        transform.LookAt(target.position);
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

public enum EnemyName
{
    Mushroom,
}
