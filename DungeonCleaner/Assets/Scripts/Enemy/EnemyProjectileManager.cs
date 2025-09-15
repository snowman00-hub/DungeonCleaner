using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileManager : MonoBehaviour
{
    public static EnemyProjectileManager Instance { get; private set; }
    public List<GameObject> projectiles;

    private Dictionary<EnemyProjectileName, Queue<GameObject>> projectilePools = new Dictionary<EnemyProjectileName, Queue<GameObject>>();
    private int addPoolSize = 100;

    private void Awake()
    {
        Instance = this;

        foreach (var projectile in projectiles)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < addPoolSize; i++)
            {
                GameObject obj = Instantiate(projectile.gameObject, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);

                var pjt = obj.GetComponent<EnemyProjectile>();
                pjt.OnUsed += () => queue.Enqueue(obj);
            }

            projectilePools[projectile.GetComponent<EnemyProjectile>().projectilename] = queue;
        }
    }

    public void Fire(EnemyProjectileName name, Vector3 startPosition, Transform target, int dmg, float speed)
    {
        var go = projectilePools[name].Dequeue();
        var pjt = go.GetComponent<EnemyProjectile>();
        pjt.SetFireInfo(startPosition, target, dmg, speed);
        go.SetActive(true);

        if (projectilePools[name].Count == 0)
        {
            for (int i = 0; i < addPoolSize; i++)
            {
                GameObject obj = Instantiate(go, transform);
                obj.SetActive(false);
                projectilePools[name].Enqueue(obj);

                var projet = obj.GetComponent<EnemyProjectile>();
                projet.OnUsed += () => projectilePools[name].Enqueue(obj);
            }
        }
    }
}