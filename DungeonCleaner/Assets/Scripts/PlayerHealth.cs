using System;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerHealth : LivingEntity
{
    //private static readonly int hashDie = Animator.StringToHash("Die");

    public event Action OnHurt;

    //public AudioClip hurtClip;
    //public AudioClip deathClip;
    //private Animator animator;
    //private AudioSource audioSource;

    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);
        OnHurt?.Invoke();
        //audioSource.PlayOneShot(hurtClip);
    }

    protected override void Die()
    {
        base.Die();

        //animator.SetTrigger(hashDie);
        //audioSource.PlayOneShot(deathClip);
    }
}
