using System;
using System.Collections;
using UnityEngine;

public enum EnemyProjectileName
{
    Scorn,
}

public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector]
    public int damage;
    public EnemyProjectileName projectilename;

    private Vector3 dir;
    private float moveSpeed;

    public Action OnUsed;

    private void Awake()
    {
        OnUsed += () => gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(CoExist());
    }

    private IEnumerator CoExist()
    {
        yield return new WaitForSeconds(10f);        
        OnUsed?.Invoke();
    }

    private void Update()
    {
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public void SetFireInfo(Vector3 startPosition, Transform target, int dmg, float speed)
    {
        transform.position = startPosition;
        dir = target.position - transform.position;
        damage = dmg;
        moveSpeed = speed;
    }
}
