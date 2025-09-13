using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class SkillArrow : MonoBehaviour
{
    public int damage = 30;
    public float speed = 5f;
    public float radius = 30f;
    public float existTime = 5f;
    public LayerMask targetLayer;

    private Vector3 dir;
    private CapsuleCollider capsule;

    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        SetDirection();
        Destroy(gameObject, existTime);
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

        foreach (var hit in hits)
        {
            hit.GetComponent<Enemy>()?.OnDamage(damage, hit.ClosestPoint(transform.position), (hit.transform.position - transform.position).normalized);
            Destroy(gameObject);
            break;
        }
    }

    private void SetDirection()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (Collider col in colliders)
        {
            sum += col.transform.position;
            count++;
        }

        if(count == 0)
        {
            dir = Vector3.forward;
        }
        else
        {
            Vector3 targetCenter = sum / count;
            dir = (targetCenter - transform.position).normalized;
            transform.LookAt(targetCenter);
        }
    }
}
