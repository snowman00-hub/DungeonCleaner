using System.Collections;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow;

public class FinalBoss : Enemy
{
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

        if (Input.GetKeyDown(KeyCode.V))
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

        Vector3 dir = (target.position - transform.position).normalized;
        transform.LookAt(target.position);

        yield return new WaitForSeconds(dashChargeTime);

        float timer = 0f;
        while (timer < dashTime)
        {
            timer += Time.deltaTime;
            transform.position += dir * dashDistance / dashTime * Time.deltaTime;
            yield return null;
        }

        capsuleCollider.radius = 1.5f;
        isUsingSkill = false;
    }
}
