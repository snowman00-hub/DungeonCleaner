using UnityEngine;

public class SkillBubbleShield : ActiveSkill
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
        capsule.radius = skillData.radius;
        particle.transform.localScale = Vector3.one * baseRadius * skillData.radius;
    }

    private void Update()
    {
        transform.position = Player.Instance.transform.position;

        if(capsule.radius != skillData.radius)
        {
            capsule.radius = skillData.radius;
            particle.transform.localScale = Vector3.one * baseRadius * skillData.radius;
        }

        CheckCollision();
    }
}
