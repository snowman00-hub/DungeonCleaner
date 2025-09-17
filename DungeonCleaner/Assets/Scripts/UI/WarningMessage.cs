using UnityEngine;

public class WarningMessage : MonoBehaviour
{
    public float scaleAmount = 0.3f; // 변하는 크기
    public float speed = 1f;         // 속도
    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    private void Update()
    {
        float scale = 1 + Mathf.PingPong(Time.time * speed, scaleAmount);
        transform.localScale = baseScale * scale;
    }
}
