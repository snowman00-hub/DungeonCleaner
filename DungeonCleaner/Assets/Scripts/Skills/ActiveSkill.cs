using System;
using System.Collections;
using UnityEngine;

public enum SkillAttribute
{
    Instant,
    Projectile,
    PersistentProjectile,
    Aura,
    PlacedAura,
}

public class ActiveSkill : MonoBehaviour
{
    public Sprite skillSprite;
    public SkillName skillName;
    public SkillAttribute skillAttribute;
    public SkillData skillData;
    public LayerMask targetLayer;
    [HideInInspector]
    public float currentCoolDown;

    public float baseRadius;

    protected float lastAttackTime;

    protected Vector3 dir;

    public event Action OnUsed;

    protected CapsuleCollider capsule;

    protected virtual void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(CoCheckExistTime());
    }

    protected IEnumerator CoCheckExistTime()
    {
        if(skillData.duration == 0f)
            yield break;

        yield return new WaitForSeconds(skillData.duration * Player.Instance.data.activeSkillDurationMultiplier);
        OnUsed?.Invoke();
    }

    protected static readonly Vector3[] directions = new Vector3[] {
        new Vector3(1, 0, 0),    // 동
        new Vector3(-1, 0, 0),   // 서
        new Vector3(0, 0, 1),    // 북
        new Vector3(0, 0, -1),   // 남
        new Vector3(1, 0, 1).normalized,   // 북동
        new Vector3(-1, 0, 1).normalized,  // 북서
        new Vector3(1, 0, -1).normalized,  // 남동
        new Vector3(-1, 0, -1).normalized  // 남서
    };

    protected void CheckCollision()
    {
        if (lastAttackTime + skillData.tickInterval > Time.time)
            return;

        float overlapRadius = Mathf.Max(capsule.height, capsule.radius * 2) / 2f;
        Collider[] hits = Physics.OverlapSphere(capsule.bounds.center, overlapRadius, targetLayer);

        if (hits.Length == 0)
            return;

        lastAttackTime = Time.time;
        foreach (var hit in hits)
        {
            int finalDamage = Mathf.FloorToInt((skillData.damage + Player.Instance.data.atk) * Player.Instance.data.finalAttackMultiplier);
            hit.GetComponent<Enemy>()?.OnDamage(finalDamage, hit.ClosestPoint(transform.position), (hit.transform.position - transform.position).normalized);
        }
    }

    protected void SetDirection()
    {
        var colliders = Physics.OverlapSphere(transform.position, skillData.projectileSpeed * skillData.duration, targetLayer);
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (Collider col in colliders)
        {
            sum += col.transform.position;
            count++;
        }

        if (count == 0)
        {
            dir = directions[UnityEngine.Random.Range(0, directions.Length)];
        }
        else
        {
            Vector3 targetCenter = sum / count;
            dir = (targetCenter - transform.position).normalized;
            transform.LookAt(targetCenter);
        }
    }

    public string GetSkillLevelId(int level)
    {
        return $"{skillName}{level}";
    }
}
