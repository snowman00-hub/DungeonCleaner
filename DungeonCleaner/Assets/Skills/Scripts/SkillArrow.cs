using System.Collections;
using UnityEngine;

public class SkillArrow : MonoBehaviour
{
    public float damage = 30f;
    public float speed = 5f;
    public float radius = 30f;
    public float existTime = 4f;
    public LayerMask targetLayer;

    private Vector3 dir;

    private void Start()
    {
        SetDirection();
        StartCoroutine(CoDestoryAfterExistTime());
    }

    private IEnumerator CoDestoryAfterExistTime()
    {
        yield return new WaitForSeconds(existTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            var enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = (other.transform.position - transform.position).normalized;
            enemyHealth.OnDamage(damage, hitPoint,hitNormal);

            Destroy(gameObject);
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
