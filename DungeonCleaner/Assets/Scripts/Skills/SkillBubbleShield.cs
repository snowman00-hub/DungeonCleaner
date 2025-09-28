using UnityEngine;

public class SkillBubbleShield : ActiveSkill
{
    public ParticleSystem particle;
    public ParticleSystem awakeningParticle;

    protected override void OnEnable()
    {
        base.OnEnable();
        capsule.radius = skillData.radius;
        particle.transform.localScale = Vector3.one * baseRadius * skillData.radius;
    }

    private bool isUpgrade = false;

    private void Update()
    {
        transform.position = Player.Instance.transform.position;

        if(skillData.skillLevel == 6 && !isUpgrade)
        {
            isUpgrade = true;
            particle.gameObject.SetActive(false);
            awakeningParticle.gameObject.SetActive(true);
            particle = awakeningParticle;
        }

        if(capsule.radius != skillData.radius)
        {
            capsule.radius = skillData.radius;
            particle.transform.localScale = Vector3.one * baseRadius * skillData.radius;
        }

        CheckCollision();
    }
}
