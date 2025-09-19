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

        // 고치기
        capsule.radius = skillData.radius;
        particle.transform.localScale = Vector3.one * baseRadius * skillData.radius;

        SetDirection();
        particle.Play();
    }

    private void Update()
    {
        transform.position += dir * skillData.projectileSpeed * Time.deltaTime;
        CheckCollision();
    }
}
