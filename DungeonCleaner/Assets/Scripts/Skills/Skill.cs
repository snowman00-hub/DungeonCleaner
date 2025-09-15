using System;
using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName = "dustStorm";
    public int damage = 10;
    public float existTime = 3f;
    public LayerMask targetLayer;

    public float coolDown = 2f;
    [HideInInspector]
    public float currentCoolDown;

    public event Action OnUsed;

    protected CapsuleCollider capsule;

    protected virtual void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(CoCheckExistTime());
    }

    protected IEnumerator CoCheckExistTime()
    {
        yield return new WaitForSeconds(existTime);
        OnUsed?.Invoke();
    }

    protected static readonly Vector3[] directions = new Vector3[] {
        new Vector3(1, 0, 0),    // 동
        new Vector3(-1, 0, 0),   // 서
        new Vector3(0, 0, 1),    // 북
        new Vector3(0, 0, -1),   // 남
        new Vector3(1, 0, 1).normalized,   // 북동
        new Vector3(-1, 0, 1).normalized,  // 북서
        new Vector3(1, 0, -1).normalized,  // 남동
        new Vector3(-1, 0, -1).normalized  // 남서
    };
}
