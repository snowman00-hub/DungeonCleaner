using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    private static readonly string Run = "Run";
    private static readonly string Idle = "Idle";

    public Transform player;
    public VirtualJoystick joystick;
    public PlayerData data;

    public Slider healthSlider;
    public event Action OnHurt;

    private float lastHurtTime;
    private float hurtTick = 0.3f;

    private Animation anim;

    private void Awake()
    {
        if (SaveLoadManager.Load())
        {
            data.maxHP = SaveLoadManager.Data.maxHP;
            data.speed = SaveLoadManager.Data.speed;
            data.pickUpRadius = SaveLoadManager.Data.pickUpRadius;
        }

        anim = GetComponentInChildren<Animation>();
        anim.wrapMode = WrapMode.Loop;
        maxHP = data.maxHP;
    }

    private void Update()
    {
        UpdateMove();
        PickUpNearbyItems();
    }

    private void LateUpdate()
    {
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        healthSlider.maxValue = maxHP;
        healthSlider.value = HP;
        healthSlider.transform.position = transform.position + Vector3.back;
        healthSlider.transform.rotation = Quaternion.LookRotation(healthSlider.transform.position - Camera.main.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.SetTriggerAttack();

            if (lastHurtTime + hurtTick > Time.time)
                return;

            lastHurtTime = Time.time;
            OnDamage(enemy.enemyData.damage, other.ClosestPoint(transform.position), (other.transform.position - transform.position).normalized);
        }

        if (other.CompareTag(Tag.EnemyAttack))
        {
            var projectile = other.GetComponent<EnemyProjectile>();
            projectile.OnUsed?.Invoke();
            OnDamage(projectile.damage, other.ClosestPoint(transform.position), (other.transform.position - transform.position).normalized);
        }

        if (other.CompareTag(Tag.Exp))
        {
            var pickup = other.GetComponent<PickUp>();
            StageInfoManager.Instance.AddExp(pickup.value);
            pickup.OnUsed?.Invoke();
        }
    }

    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);
        OnHurt?.Invoke();
    }

    protected override void Die()
    {
        base.Die();
    }

    private void PickUpNearbyItems()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, data.pickUpRadius, LayerMask.GetMask(LayerName.PickUp));
        foreach (var pickup in nearby)
        {
            pickup.gameObject.GetComponent<PickUp>().Acquire(transform);
        }
    }

    private void UpdateMove()
    {
        Vector2 input = new Vector2(joystick.Input.x, joystick.Input.y);
        Vector3 move = new Vector3(input.x, 0, input.y);
        transform.position += move * data.speed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(move);

            if (!anim.IsPlaying(Run))
            {
                anim.Play(Run);
            }
        }
        else
        {
            if (!anim.IsPlaying(Idle))
            {
                anim.CrossFade(Idle, 0.3f);
            }
        }
    }
}
