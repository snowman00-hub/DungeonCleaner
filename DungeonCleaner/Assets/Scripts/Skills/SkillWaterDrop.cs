using System.Collections;
using UnityEngine;

public class SkillWaterDrop : ActiveSkill
{
    public GameObject defaultParticle;
    public GameObject awakeningParticle;
    public GameObject defaultExplodeGo;
    public ParticleSystem explodeParticle;
    public ParticleSystem awakeningExplode;

    private SphereCollider sphereCollider;

    private bool isUsed = false;

    protected override void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if(skillData.skillLevel == 6)
        {
            defaultExplodeGo.SetActive(false);
            defaultParticle.SetActive(false);
            awakeningParticle.SetActive(true);
            explodeParticle = awakeningExplode;
        }
        else
        {
            defaultParticle.SetActive(true);
        }

        sphereCollider.enabled = true;
        isUsed = false;

        SetDirection(25f);

        explodeParticle.gameObject.transform.localScale = Vector3.one / 2f * skillData.radius;
        explodeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void Update()
    {
        if (isUsed)
            return;

        transform.position += dir * skillData.projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            var hits = Physics.OverlapSphere(transform.position, skillData.radius, targetLayer);
            foreach (var hit in hits)
            {
                int finalDamage = Mathf.FloorToInt((skillData.damage + Player.Instance.data.atk) * Player.Instance.data.finalAttackMultiplier);
                hit.GetComponent<Enemy>()?.OnDamage(finalDamage, hit.ClosestPoint(transform.position), (hit.transform.position - transform.position).normalized);
            }

            StartCoroutine(CoExplode());
        }
    }

    private IEnumerator CoExplode()
    {
        sphereCollider.enabled = false;
        defaultParticle.SetActive(false);
        isUsed = true;
        explodeParticle.Play();
        yield return new WaitForSeconds(explodeParticle.main.duration);
        OnUsed?.Invoke();
    }
}
