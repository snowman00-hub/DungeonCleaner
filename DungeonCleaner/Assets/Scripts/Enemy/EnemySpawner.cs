using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int stageNumber = 1;
    public List<GameObject> normalMonsters;
    public List<GameObject> miniBossMonsters;
    public GameObject bossMonster;

    private Transform player;
    private Dictionary<EnemyName, Queue<GameObject>> monsterPools = new Dictionary<EnemyName, Queue<GameObject>>();

    private List<SpawnData> spawnDatas;

    private void Awake()
    {
        Variables.CurrentStageNumber = stageNumber;
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
                        SpawnEnemy(data.MON_NAME, data.MON_COUNT, minRadius, maxRadius,data.IS_SWARM);
                }

                yield return new WaitForSeconds(data.INTERVAL);
            }
        }
        else
        {
            while (StageInfoManager.Instance.gameTimer < data.START_TIME)
                yield return null;

            if (data.MON_TYPE == EnemyType.Boss)
            {
                var spawnPos = Player.Instance.transform.position + new Vector3(0, 0, 18);
                var monster = monsterPools[data.MON_NAME].Dequeue();
                monster.transform.position = spawnPos;
                monster.SetActive(true);
                yield break;
            }

            if (Random.Range(0f, 100f) <= data.WEIGHT)
                SpawnEnemy(data.MON_NAME, data.MON_COUNT, minRadius, maxRadius, data.IS_SWARM);

            yield break;
        }
    }

    public void SpawnEnemy(EnemyName name, int count, float minRadius, float maxRadius, bool isSwarm)
    {
        if (!monsterPools.ContainsKey(name))
        {
            Debug.Log("몬스터 키 없음");
            return;
        }

        if (isSwarm)
        {
            var spawnPos = MyUtils.GetRandomPositionInRing3D(player.position, minRadius, maxRadius);
            for (int i = 0; i < count; i++)
            {
                var monster = monsterPools[name].Dequeue();
                monster.transform.position = spawnPos;
                monster.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                var spawnPos = MyUtils.GetRandomPositionInRing3D(player.position, minRadius, maxRadius);
                var monster = monsterPools[name].Dequeue();
                monster.transform.position = spawnPos;
                monster.SetActive(true);
            }
        }

        if (monsterPools[name].Count < 100)
        {
            var go = monsterPools[name].Peek();
            ExpandMonsterPool(name, go, 100);
        }
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
            enemyData.dropItem1 = csvData.DROP_ITEM1;
            enemyData.dropItemValue1 = csvData.DROP_ITEM_VALUE1;
            enemyData.dropItem2 = csvData.DROP_ITEM2;
            enemyData.dropItemValue2 = csvData.DROP_ITEM_VALUE2;
            enemyData.dropItem3 = csvData.DROP_ITEM3;
            enemyData.dropItemValue3 = csvData.DROP_ITEM_VALUE3;
            enemyData.bossType = csvData.BMON_TYPE;

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
            enemyData.dropItem1 = csvData.DROP_ITEM1;
            enemyData.dropItemValue1 = csvData.DROP_ITEM_VALUE1;
            enemyData.dropItem2 = csvData.DROP_ITEM2;
            enemyData.dropItemValue2 = csvData.DROP_ITEM_VALUE2;
            enemyData.dropItem3 = csvData.DROP_ITEM3;
            enemyData.dropItemValue3 = csvData.DROP_ITEM_VALUE3;
            enemyData.bossType = csvData.BMON_TYPE;

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
            };

            monsterPools[name].Enqueue(go);
        }
    }
}
