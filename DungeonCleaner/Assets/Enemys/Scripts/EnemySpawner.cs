using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float minRadius = 5f;
    public float maxRadius = 7f;
    public int spawnCount = 10;
    public List<GameObject> normalMonsters;
    public List<GameObject> miniBossMonsters;
    public GameObject bossMonster;

    private Transform player;
    private Dictionary<EnemyName, Queue<GameObject>> monsterPools = new Dictionary<EnemyName, Queue<GameObject>>();
    private int normalMonsterCount = 0;

    private void Start()
    {
        player = GameObject.FindWithTag(Tag.Player).transform;
        Init();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy(EnemyName.Mushroom, spawnCount, minRadius, maxRadius);
        }
#endif
    }

    public void SpawnEnemy(EnemyName name, int count, float minRadius, float maxRadius)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPos = GetRandomPositionInRing3D(player.position, minRadius, maxRadius);
            var monster = monsterPools[name].Dequeue();
            monster.transform.position = spawnPos;
            monster.SetActive(true);
            normalMonsterCount++;
        }

        if(monsterPools[name].Count < 100)
        {
            var go = monsterPools[name].Peek();
            ExpandMonsterPool(name, go);
        }
    }

    private Vector3 GetRandomPositionInRing3D(Vector3 center, float minRadius, float maxRadius)
    {
        float radius = Random.Range(minRadius, maxRadius);
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);

        return new Vector3(x, center.y, z);
    }

    private void Init()
    {
        foreach (var monster in normalMonsters)
        {
            var name = monster.GetComponent<EnemyHealth>().enemyName;
            monsterPools[name] = new Queue<GameObject>();
            ExpandMonsterPool(name, monster);
        }
    }

    private void ExpandMonsterPool(EnemyName name, GameObject monster)
    {
        for (int i = 0; i < 100; i++)
        {
            var go = Instantiate(monster, transform);
            go.SetActive(false);

            var enemyHealth = go.GetComponent<EnemyHealth>();
            enemyHealth.OnDeath += () =>
            {
                monsterPools[name].Enqueue(go);
                go.SetActive(false);
                normalMonsterCount--;
            };

            monsterPools[name].Enqueue(go);
        }
    }
}
