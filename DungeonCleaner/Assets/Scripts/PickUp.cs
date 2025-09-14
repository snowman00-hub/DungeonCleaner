using System;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int value = 10;
    public float bounceDistance = 3f;
    public float bounceBackTime = 0.25f;
    public float moveTime = 0.5f;
    public PickUpType type;

    private float timer;
    private Vector3 reverseDir;

    private float speed;
    private Transform player;
    private bool isAcquired = false;

    private bool isPulling = false;
    private Vector3 pullStartPos;
    private float pullStartTime;

    public Action OnUsed;

    private void OnEnable()
    {
        isAcquired = false;
    }

    private void Update()
    {
        if (!isAcquired)
            return;

        if (timer + bounceBackTime > Time.time)
        {
            transform.position += reverseDir * speed * Time.deltaTime;
        }
        else
        {
            if (!isPulling)
            {
                isPulling = true;
                pullStartPos = transform.position;
                pullStartTime = Time.time;
            }

            float t = (Time.time - pullStartTime) / moveTime; 
            transform.position = Vector3.Lerp(pullStartPos, player.position, t);
        }
    }

    public void Acquire(Transform target)
    {
        if (isAcquired)
            return;

        isAcquired = true;
        isPulling = false;
        player = target;
        reverseDir = (transform.position - player.position).normalized;
        speed = bounceDistance / bounceBackTime;
        timer = Time.time;
    }
}
