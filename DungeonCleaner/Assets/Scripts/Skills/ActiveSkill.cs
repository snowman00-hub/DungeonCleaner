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
    public Sprite awakeningSkillSprite;
    public SkillName skillName;
    public SkillAttribute skillAttribute;
    public SkillData skillData;
    public LayerMask targetLayer;
    [HideInInspector]
    public float currentCoolDown;

    public float baseRadius;

    protected float lastAttackTime;

    protected Vector3 dir;

    public Action OnUsed;

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
        if (skillData.duration == 0f)
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

    private Collider[] hitBuffer = new Collider[20];

    protected void CheckCollision()
    {
        if (lastAttackTime + skillData.tickInterval > Time.time)
            return;

        float overlapRadius = Mathf.Max(capsule.height, capsule.radius * 2) / 2f;

        int hitCount = Physics.OverlapSphereNonAlloc(capsule.bounds.center, overlapRadius, hitBuffer, targetLayer);
        if (hitCount == 0)
            return;

        lastAttackTime = Time.time;

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = hitBuffer[i];
            if (hit == null)
                continue;

            int finalDamage = Mathf.FloorToInt((skillData.damage + Player.Instance.data.atk) * Player.Instance.data.finalAttackMultiplier);
            hit.GetComponent<Enemy>()?.OnDamage(finalDamage, hit.ClosestPoint(transform.position), (hit.transform.position - transform.position).normalized);
            ActiveSkillManager.Instance.damageAmounts[skillName] += finalDamage;
        }
    }
    
    private Collider[] checkBuffer = new Collider[100];

    protected void SetDirection(float checkRadius)
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, checkRadius, checkBuffer, targetLayer);
        Vector3 weightedSum = Vector3.zero;
        float totalWeight = 0f;

        for(int i = 0; i < count; i++)
        {
            Collider col = checkBuffer[i];
            if(col == null)
                continue;
            float dist = Vector3.Distance(transform.position, col.transform.position);

            // 거리에 따른 가중치
            float weight = 1f / (dist + 0.001f);

            weightedSum += col.transform.position * weight;
            totalWeight += weight;
        }

        if (totalWeight == 0)
        {
            dir = directions[UnityEngine.Random.Range(0, directions.Length)];
            transform.LookAt(transform.position + dir);
        }
        else
        {
            Vector3 targetCenter = weightedSum / totalWeight;
            dir = (targetCenter - transform.position).normalized;
            transform.LookAt(targetCenter);
        }
    }

    public string GetSkillLevelId(int level)
    {
        return $"{skillName}{level}";
    }
}
