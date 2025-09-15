using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum EnemyName
{
    smallMushroom,
    smallBat,
    smallSpider,
    mediumSpider,
    bigSpider,
    earthGolem
}

public enum EnemyType
{
    None,
    Normal,
    MiniBoss,
    Boss
}

public class EnemySpawner : MonoBehaviour
{
    public float minRadius = 7f;
    public float maxRadius = 10f;
    public List<GameObject> normalMonsters;
    public List<GameObject> miniBossMonsters;
    public GameObject bossMonster;

    private Transform player;
    private Dictionary<EnemyName, Queue<GameObject>> monsterPools = new Dictionary<EnemyName, Queue<GameObject>>();
    private int normalMonsterCount = 0;

    private List<SpawnData> spawnDatas;

    private void Awake()
    {
        spawnDatas = DataTableManger.SpawnTable.GetList();
    }

    private void Start()
    {
        player = GameObject.FindWithTag(Tag.Player).transform;
        Init();
        StartCoroutines();
    }

    private void StartCoroutines()
    {
        foreach (var spawnData in spawnDatas)
        {
            StartCoroutine(CoSpawn(spawnData));
        }
    }

    private IEnumerator CoSpawn(SpawnData data)
    {
        if (data.MON_TYPE == EnemyType.Normal)
        {
            while (true)
            {
                float t = StageInfoManager.Instance.gameTimer;

                if (t >= data.START_TIME && t < data.END_TIME)
                {
                    if (Random.Range(0f, 100f) <= data.WEIGHT)
                        SpawnEnemy(data.MON_NAME, data.MON_COUNT, minRadius, maxRadius);
                }

                yield return new WaitForSeconds(data.INTERVAL);
            }
        }
        else
        {
            while (StageInfoManager.Instance.gameTimer < data.START_TIME)
                yield return null;

            if (Random.Range(0f, 100f) <= data.WEIGHT)
                SpawnEnemy(data.MON_NAME, data.MON_COUNT, minRadius, maxRadius);

            yield break;
        }
    }

    public void SpawnEnemy(EnemyName name, int count, float minRadius, float maxRadius)
    {
        if (!monsterPools.ContainsKey(name))
        {
            Debug.Log("몬스터 키 없음");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            var spawnPos = GetRandomPositionInRing3D(player.position, minRadius, maxRadius);
            var monster = monsterPools[name].Dequeue();
            monster.transform.position = spawnPos;
            monster.SetActive(true);
            normalMonsterCount++;
        }

        if (monsterPools[name].Count < 100)
        {
            var go = monsterPools[name].Peek();
            ExpandMonsterPool(name, go, 100);
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
            var enemy = monster.GetComponent<Enemy>();

            var csvData = DataTableManger.MonsterTable.Get(enemy.enemyName);
            enemy.enemyData.damage = csvData.ATK;
            enemy.enemyData.maxHp = csvData.MAXHP;
            enemy.enemyData.moveSpeed = csvData.MOVE_SPEED;
            enemy.enemyData.dropExp = csvData.DROP_EXP;
            enemy.enemyData.dropPercent = csvData.DROP_PER;
            enemy.enemyData.projectileRange = csvData.PROJECTILE_RANGE;
            enemy.enemyData.projectileCooldown = csvData.PROJECTILE_COOLTIME;
            enemy.enemyData.projectileSpeed = csvData.PROJECTILE_MOVE_SPEED;

            monsterPools[enemy.enemyName] = new Queue<GameObject>();
            ExpandMonsterPool(enemy.enemyName, monster, 100);
        }

        foreach (var miniBoss in miniBossMonsters)
        {
            var enemy = miniBoss.GetComponent<Enemy>();

            var csvData = DataTableManger.BossMonsterTable.Get(enemy.enemyName);
            var enemyData = enemy.enemyData as BossEnemyData;
            enemyData.damage = csvData.ATK;
            enemyData.maxHp = csvData.MAXHP;
            enemyData.moveSpeed = csvData.MOVE_SPEED;
            enemyData.dropExp = csvData.DROP_EXP;
            enemyData.dropPercent = csvData.DROP_PER;
            enemyData.projectileRange = csvData.PROJECTILE_RANGE;
            enemyData.projectileCooldown = csvData.PROJECTILE_COOLTIME;
            enemyData.projectileSpeed = csvData.PROJECTILE_MOVE_SPEED;
            enemyData.projectile_count = csvData.PROJECTILE_COUNT;
            enemyData.dropItem = csvData.DROP_ITEM;
            enemyData.dropItemValue = csvData.DROP_ITEM_VALUE;

            monsterPools[enemy.enemyName] = new Queue<GameObject>();
            ExpandMonsterPool(enemy.enemyName, miniBoss, 10);
        }

        {
            var boss = bossMonster.GetComponent<Enemy>();

            var csvData = DataTableManger.BossMonsterTable.Get(boss.enemyName);
            var enemyData = boss.enemyData as BossEnemyData;
            enemyData.damage = csvData.ATK;
            enemyData.maxHp = csvData.MAXHP;
            enemyData.moveSpeed = csvData.MOVE_SPEED;
            enemyData.dropExp = csvData.DROP_EXP;
            enemyData.dropPercent = csvData.DROP_PER;
            enemyData.projectileRange = csvData.PROJECTILE_RANGE;
            enemyData.projectileCooldown = csvData.PROJECTILE_COOLTIME;
            enemyData.projectileSpeed = csvData.PROJECTILE_MOVE_SPEED;
            enemyData.projectile_count = csvData.PROJECTILE_COUNT;
            enemyData.dropItem = csvData.DROP_ITEM;
            enemyData.dropItemValue = csvData.DROP_ITEM_VALUE;

            monsterPools[boss.enemyName] = new Queue<GameObject>();
            ExpandMonsterPool(boss.enemyName, bossMonster, 2);
        }
    }

    private void ExpandMonsterPool(EnemyName name, GameObject monster, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(monster, transform);
            go.SetActive(false);

            var enemyHealth = go.GetComponent<Enemy>();
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
