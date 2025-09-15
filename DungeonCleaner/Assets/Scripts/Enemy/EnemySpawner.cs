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
    public DataLoadType loadType;
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

            if (loadType == DataLoadType.ScriptableObject)
            {

            }
            else if (loadType == DataLoadType.CSV)
            {

            }

            monsterPools[enemy.enemyName] = new Queue<GameObject>();
            ExpandMonsterPool(enemy.enemyName, monster, 100);
        }

        foreach (var miniBoss in miniBossMonsters)
        {
            var name = miniBoss.GetComponent<Enemy>().enemyName;
            monsterPools[name] = new Queue<GameObject>();
            ExpandMonsterPool(name, miniBoss, 10);
        }

        var bossName = bossMonster.GetComponent<Enemy>().enemyName;
        monsterPools[bossName] = new Queue<GameObject>();
        ExpandMonsterPool(bossName, bossMonster, 2);
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
