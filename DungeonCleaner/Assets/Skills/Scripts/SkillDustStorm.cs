using System;
using System.Collections;
using UnityEngine;

public class SkillDustStorm : Skill
{
    public float speed = 2f;
    public float radius = 3f;

    private float findEnemyRadius = 30f;
    private Vector3 dir;

    private float lastAttackTime;
    [HideInInspector]
    public float attackTick = 0.5f;

    private ParticleSystem particle;

    protected override void Awake()
    {
        base.Awake();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var mainModule = particle.main;
        mainModule.duration = existTime;

        capsule.radius = radius;
        particle.transform.localScale = Vector3.one * radius / 3f;

        SetDirection();
        particle.Play();
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        UpdateCollision();
    }

    private void UpdateCollision()
    {
        float overlapRadius = Mathf.Max(capsule.height, capsule.radius * 2) / 2f;
        Collider[] hits = Physics.OverlapSphere(capsule.bounds.center, overlapRadius, targetLayer);

        if (hits.Length == 0 || lastAttackTime + attackTick > Time.time)
            return;

        lastAttackTime = Time.time;
        foreach (var hit in hits)
        {
            hit.GetComponent<Enemy>()?.OnDamage(damage, hit.ClosestPoint(transform.position), (hit.transform.position - transform.position).normalized);
        }
    }

    private void SetDirection()
    {
        var colliders = Physics.OverlapSphere(transform.position, findEnemyRadius, targetLayer);
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
}
