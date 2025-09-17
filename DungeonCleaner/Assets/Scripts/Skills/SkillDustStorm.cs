using System;
using System.Collections;
using UnityEngine;

public class SkillDustStorm : ActiveSkill
{
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
        mainModule.duration = skillData.duration;

        capsule.radius = skillData.radius;
        particle.transform.localScale = Vector3.one * skillData.radius;

        SetDirection();
        particle.Play();
    }

    private void Update()
    {
        transform.position += dir * skillData.projectileSpeed * Time.deltaTime;
        UpdateCollision();
    }

    private void UpdateCollision()
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
}
