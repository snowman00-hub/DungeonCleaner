using UnityEngine;

public interface IDamagable
{
    void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal);
}