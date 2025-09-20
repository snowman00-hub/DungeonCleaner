using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FinalBoss : Enemy
{
    public static readonly int hashDashStart = Animator.StringToHash("DashStart");
    public static readonly int hashDashEnd = Animator.StringToHash("DashEnd");

    public GameObject dashLine;

    public float dashCoolDown = 25f;
    public float dashChargeTime =1f;
    public float dashTime = 2f;
    public float dashDistance = 40f;

    public int fireCount;
    public float fireCoolDown;
    public float fireInterval;
    public float fireChargeTime;

    private bool isUsingSkill;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(CoCheckDash());
    }

    protected override void Update()
    {
        if (IsDead || isUsingSkill)
            return;

        if (Input.GetKeyDown(KeyCode.V) && !isUsingSkill)
        {
            StartCoroutine(CoDash());
        }

        UpdateMove();
    }

    private IEnumerator CoCheckDash()
    {
        float timer = Time.time;
        while (true)
        {
            if(timer + dashCoolDown < Time.time && !isUsingSkill)
            {
                timer = Time.time;
                StartCoroutine(CoDash());
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
        size.y = 0;
        while(timer < dashChargeTime)
        {
            timer += Time.deltaTime;
            size.y += dashDistance * Time.deltaTime;
            Vector3 pivot = new Vector3(0, size.y / -2, 0.1f);
            decalprojector.size = size;
            decalprojector.pivot = pivot;
            yield return null;
        }

        yield return new WaitForSeconds(dashTime);
        Destroy(projector);
    }
}
