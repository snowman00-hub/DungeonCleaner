using System.Collections;
using UnityEngine;

public class OutOfMoneyMessage : MonoBehaviour
{
    public float scaleAmount = 0.3f; // 변하는 크기
    public float speed = 1f;         // 속도
    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        StartCoroutine(CoExist());
    }

    private IEnumerator CoExist()
    {
        yield return new WaitForSecondsRealtime(2f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float scale = 1 + Mathf.PingPong(Time.unscaledTime * speed, scaleAmount);
        transform.localScale = baseScale * scale;
    }
}