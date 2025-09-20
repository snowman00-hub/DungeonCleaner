using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FinalBoss : Enemy
{
    public static readonly int hashDashStart = Animator.StringToHash("DashStart");
    public static readonly int hashDashEnd = Animator.StringToHash("DashEnd");
    public static readonly int hashCasting = Animator.StringToHash("Casting");
    public static readonly int hashShoot = Animator.StringToHash("Shoot");

    public GameObject dashLine;
    public GameObject Rock;

    public float skillCoolDown = 5f;
    public float dashChargeTime =1f;
    public float dashTime = 0.5f;
    public float dashDistance = 40f;

    public int fireCount =3;
    public float fireInterval = 0.3f;
    public float fireChargeTime = 1f;
    public float fireSpeed = 10f;

    private bool isUsingSkill;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(CoCheckSkillCoolDown());
    }

    protected override void Update()
    {
        BossHpBar.Instance.hpBar.maxValue = maxHP;
        BossHpBar.Instance.hpBar.value = HP;

        if (IsDead || isUsingSkill)
            return;

        UpdateMove();
    }

    private IEnumerator CoFire()
    {
        animator.SetTrigger(hashCasting);
        yield return new WaitForSeconds(fireChargeTime);
        for(int i=0;i<fireCount;i++)
        {
            animator.SetTrigger(hashShoot);
            yield return new WaitForSeconds(0.3f);
            var rock = Instantiate(Rock).GetComponent<EnemyProjectile>();
            rock.SetFireInfo(transform.position + transform.forward, target, enemyData.damage * 2, fireSpeed);
            yield return new WaitForSeconds(fireInterval);
        }

        animator.ResetTrigger(hashAttack);
        animator.ResetTrigger(hashHurt);
    }

    private IEnumerator CoCheckSkillCoolDown()
    {
        float timer = Time.time;
        while (true)
        {
            if(timer + skillCoolDown < Time.time && !isUsingSkill)
            {
                timer = Time.time;
                var rand = Random.Range(0, 1f);
                if(rand > 0.5f)
                    StartCoroutine(CoDash());
                else
                    StartCoroutine(CoFire());
            }

            yield return null;
        }
    }

    private IEnumerator CoDash()
    {
        isUsingSkill = true;
        capsuleCollider.radius = 3f;
        enemyData.damage *= 2;
       
        Vector3 dir = (target.position - transform.position).normalized;
        transform.LookAt(target.position);
        animator.SetTrigger(hashDashStart);
        animator.ResetTrigger(hashDashEnd);  

        StartCoroutine(CoDashLine());
        yield return new WaitForSeconds(dashChargeTime);

        float timer = 0f;
        while (timer < dashTime)
        {
            timer += Time.deltaTime;
            transform.position += dir * dashDistance / dashTime * Time.deltaTime;
            yield return null;
        }

        isUsingSkill = false;
        capsuleCollider.radius = 1.5f;
        enemyData.damage /= 2;
        animator.SetTrigger(hashDashEnd);
        animator.ResetTrigger(hashAttack);
        animator.ResetTrigger(hashHurt);
    }

    private IEnumerator CoDashLine()
    {
        var projector = Instantiate(dashLine);
        projector.transform.position = transform.position;
        projector.transform.LookAt(target);

        float timer = 0f;
        var decalprojector = projector.GetComponentInChildren<DecalProjector>();
        var size = decalprojector.size;
        var pivot = new Vector3(0, 0,0.1f);
        size.y = 0;
        while(timer < dashChargeTime)
        {
            timer += Time.deltaTime;
            size.y += dashDistance * Time.deltaTime;
            pivot.y = size.y / -2f;

            decalprojector.size = size;
            decalprojector.pivot = pivot;
            yield return null;
        }

        yield return new WaitForSeconds(dashTime);
        Destroy(projector);
    }
}
