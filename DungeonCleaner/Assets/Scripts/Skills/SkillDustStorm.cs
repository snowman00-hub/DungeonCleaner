using UnityEngine;

public class SkillDustStorm : ActiveSkill
{
    public ParticleSystem defaultParticle;
    public ParticleSystem awakeningParticle;

    protected override void OnEnable()
    {
        base.OnEnable();

        if(skillData.skillLevel == 6)
        {
            defaultParticle.gameObject.SetActive(false);
            awakeningParticle.gameObject.SetActive(true);

            awakeningParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var mainModule = awakeningParticle.main;
            mainModule.duration = skillData.duration;
            awakeningParticle.transform.localScale = Vector3.one * baseRadius * skillData.radius;
            awakeningParticle.Play();
        }
        else
        {
            defaultParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var mainModule = defaultParticle.main;
            mainModule.duration = skillData.duration;
            defaultParticle.transform.localScale = Vector3.one * baseRadius * skillData.radius;
            defaultParticle.Play();
        }

        capsule.radius = skillData.radius;
        SetDirection(skillData.duration * skillData.projectileSpeed);
    }

    private void Update()
    {
        transform.position += dir * skillData.projectileSpeed * Time.deltaTime;
        CheckCollision();
    }
}
