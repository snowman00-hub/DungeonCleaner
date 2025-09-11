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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collisionEnter");
        if(collision.collider.CompareTag(Tag.Enemy))
        {            
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.OnDamage(damage, collision.contacts[0].point, collision.contacts[0].normal);
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

        Vector3 targetCenter = sum / count;
        dir = (targetCenter - transform.position).normalized;
        transform.LookAt(targetCenter);
    }
}
