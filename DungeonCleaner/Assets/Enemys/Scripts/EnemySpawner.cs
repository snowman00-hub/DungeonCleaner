using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float minRadius = 5f;
    public float maxRadius = 7f;
    public int spawnCount = 10;
    public GameObject enemyPrefab;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag(Tag.Player).transform;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy(minRadius, maxRadius, spawnCount, enemyPrefab);
        }
#endif
    }

    public void SpawnEnemy(float minRadius, float maxRadius, int count, GameObject prefab)
    {
        for(int i=0;i<count;i++)
        {
            var spawnPos = GetRandomPositionInRing3D(player.position, minRadius, maxRadius);
            Instantiate(prefab, spawnPos, Quaternion.identity, transform);
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
}
