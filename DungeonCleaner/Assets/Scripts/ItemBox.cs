using System;
using System.Collections;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public event Action OnDestroy;

    private void OnEnable()
    {
        StartCoroutine(CoExist());
    }

    private void OnTriggerEnter(Collider other)
    {
        int rand = UnityEngine.Random.Range(4, 10);
        PickUpManager.Instance.CreatePickUp((PickUpType)rand, transform.position);
        OnDestroy?.Invoke();
    }

    private IEnumerator CoExist()
    {
        yield return new WaitForSeconds(20f);
        OnDestroy?.Invoke();
    }
}