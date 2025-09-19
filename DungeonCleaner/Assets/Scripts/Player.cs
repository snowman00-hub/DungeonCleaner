using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public static Player Instance { get; private set; }

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

    public GameObject attackAura;
    public GameObject powerAura;

    private bool isPowerOn = false;

    private void Awake()
    {
        Instance = this;

        if (SaveLoadManager.Load())
        {
            data.maxHP = SaveLoadManager.Data.maxHP;
            data.atk = SaveLoadManager.Data.atk;
            data.finalAttackMultiplier = SaveLoadManager.Data.finalAttackMultiplier;
            data.finalDamageReduction = SaveLoadManager.Data.finalDamageReduction;
            data.def = SaveLoadManager.Data.def;
            data.speed = SaveLoadManager.Data.speed;
            data.activeSkillDurationMultiplier = SaveLoadManager.Data.activeSkillDurationMultiplier;
            data.pickUpRadius = SaveLoadManager.Data.pickUpRadius;
        }
        else
        {
            data.maxHP = 200;
            data.atk = 20;
            data.finalAttackMultiplier = 1f;
            data.finalDamageReduction = 0f;
            data.def = 5;
            data.speed = 7;
            data.activeSkillDurationMultiplier = 1f;
            data.pickUpRadius = 2f;
        }

        anim = GetComponentInChildren<Animation>();
        anim.wrapMode = WrapMode.Loop;
        maxHP = data.maxHP;
        data.InitialMaxHP = data.maxHP;
        data.InitialSpeed = data.speed;
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

    private void OnTriggerStay(Collider other)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.EnemyAttack))
        {
            var projectile = other.GetComponent<EnemyProjectile>();
            projectile.OnUsed?.Invoke();
            OnDamage(projectile.damage, other.ClosestPoint(transform.position), (other.transform.position - transform.position).normalized);
        }

        if (other.CompareTag(Tag.Exp) || other.CompareTag(Tag.Item))
        {
            var pickup = other.GetComponent<PickUp>();
            pickup.TakeEffect();

            switch (pickup.type)
            {
                case PickUpType.smallExp:
                case PickUpType.mediumExp:
                case PickUpType.largeExp:
                    AudioManager.Instance.ExpGet(transform.position);
                    break;
                case PickUpType.smallGold:
                case PickUpType.mediumGold:
                case PickUpType.largeGold:
                    AudioManager.Instance.GoldGet(transform.position);
                    break;
                case PickUpType.food:
                    AudioManager.Instance.FoodGet(transform.position);
                    break;
                case PickUpType.magnet:
                    break;
                case PickUpType.bomb:
                    break;
                case PickUpType.expPotion:
                    break;
                case PickUpType.atkPotion:
                    break;
                case PickUpType.invinciblePotion:
                    break;
            }
        }
    }

    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead || isPowerOn)
            return;

        int finalDamage = Mathf.FloorToInt((damage - data.def) * (1f - data.finalDamageReduction));
        base.OnDamage(finalDamage, hitPoint, hitNormal);
        OnHurt?.Invoke();
        Handheld.Vibrate();
    }

    protected override void Die()
    {
        base.Die();
        StageInfoManager.Instance.Defeat();
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
        Vector3 move = new Vector3(input.x, 0, input.y) * data.speed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            Vector3 rayPos = transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(rayPos, move.normalized, out RaycastHit hit, 1f, LayerMask.GetMask(LayerName.Wall)))
            {
                move = Vector3.ProjectOnPlane(move, hit.normal);
            }

            transform.position += move;

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

    public void MaxHpUp(int add)
    {
        data.maxHP += add;
        maxHP = data.maxHP;
        HP += add;
    }

    public void Heal(int amount)
    {
        HP += amount;
        if (HP > maxHP)
            HP = maxHP;
    }

    public void UsePotion(StorePotion potion)
    {
        switch (potion.potionType)
        {
            case PotionType.atkPotion:
                StartCoroutine(CoAttackAuraOn(potion.value));
                break;
            case PotionType.expPotion:
                for (int i = 0; i < 20; i++)
                {
                    PickUpManager.Instance.CreatePickUp(PickUpType.largeExp, MyUtils.GetRandomPositionInRing3D(transform.position, 2f, 3f));
                }
                break;
            case PotionType.powerPotion:
                StartCoroutine(CoPowerAuraOn(potion.value));
                break;
        }
    }

    private IEnumerator CoAttackAuraOn(float value)
    {
        attackAura.SetActive(true);
        data.finalAttackMultiplier += value / 100;
        yield return new WaitForSeconds(30f);
        data.finalAttackMultiplier -= value / 100;
        attackAura.SetActive(false);
    }

    private IEnumerator CoPowerAuraOn(float value)
    {
        powerAura.SetActive(true);
        isPowerOn = true;
        yield return new WaitForSeconds(value);
        isPowerOn = false;
        powerAura.SetActive(false);
    }


    public void BombAttack(float bombRadius)
    {
        Collider[] enemys = Physics.OverlapSphere(transform.position, bombRadius, LayerMask.GetMask(LayerName.Enemy));

        foreach (var monster in enemys)
        {
            var enemy = monster.gameObject.GetComponent<Enemy>();
            if (enemy.enemyData is BossEnemyData)
            {
                enemy.OnDamage(Mathf.FloorToInt(enemy.maxHP * 0.2f), enemy.transform.position, transform.position);
            }
            else
            {
                enemy.OnDamage(enemy.maxHP, enemy.transform.position, transform.position);
            }
        }

        StageInfoManager.Instance.StartBombFlash();
    }
}
