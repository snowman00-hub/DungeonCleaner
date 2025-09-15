using System.Collections;
using UnityEngine;

public enum EnemyAttackType
{
    Melee = 1,
    Ranged = 2,
}

public class Enemy : LivingEntity
{
    public static readonly int hashAttack = Animator.StringToHash("Attack");
    public static readonly int hashHurt = Animator.StringToHash("Hurt");
    public static readonly int hashDie = Animator.StringToHash("Die");

    public int damage = 10;
    public float speed = 5f;
    public float avoidWeight = 0.5f;    

    public PickUpType expType = PickUpType.smallExp;
    public float expDropRate = 70f;

    public EnemyAttackType atkType = EnemyAttackType.Melee;
    protected float projectileRange = 5f;
    protected float projectileCoolDown = 2f;
    protected float projectileSpeed = 3f;

    public EnemyName enemyName;
    public LayerMask enemyMask;

    protected Transform target;
    protected CapsuleCollider capsuleCollider;
    protected float timer;
    protected float checkRemoveTime = 10f;

    protected Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        target = GameObject.FindWithTag(Tag.Player).transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        capsuleCollider.enabled = true;
    }

    protected virtual void Update()
    {
        if (IsDead)
            return;

        UpdateMove();

        if (timer + checkRemoveTime < Time.time)
        {
            timer = Time.time;
            if (Vector3.Distance(transform.position, target.position) > 200f)
            {
                Die();
            }
        }
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

    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);
        animator.SetTrigger(hashHurt);
        DamagePopupManager.Instance.ShowDamage(hitPoint, Mathf.FloorToInt(damage));
    }

    protected override void Die()
    {
        StartCoroutine(CoDie());
    }

    private IEnumerator CoDie()
    {
        capsuleCollider.enabled = false;
        IsDead = true;
        animator.SetTrigger(hashDie);
        yield return new WaitForSeconds(1.1f);
        base.Die();
        StageInfoManager.Instance.KillCount++;
        PickUpManager.Instance.CreatePickUp(expType, transform.position);
    }

    public void SetTriggerAttack()
    {
        animator.SetTrigger(hashAttack);
    }
}

