using System.Collections.Generic;
using UnityEngine;

public class InfiniteMapGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    public float chunkWidth = 30f;
    public float chunkHeight = 30f;

    private Transform player;

    private const int chunkPoolCount = 50;
    private Queue<GameObject> chunkPool = new Queue<GameObject>();
    private Dictionary<(int, int), GameObject> chunks = new Dictionary<(int, int), GameObject>();
    private (int x, int z) currentCoordinate = (0, 0);

    private float lastRemoveTime;
    private float RemoveInterval = 2f;
    private const int removeDistance = 5;

    private void Start()
    {
        player = GameObject.FindWithTag(Tag.Player).transform;
        Init();
    }

    private void Update()
    {
        UpdateCreateChunk();

        if (lastRemoveTime + RemoveInterval < Time.time)
        {
            lastRemoveTime = Time.time;
            UpdateRemoveChunk();
        }
    }

    private void UpdateRemoveChunk()
    {
        var keysToRemove = new List<(int, int)>();

        foreach (var chunk in chunks)
        {
            int distance = Mathf.Abs(currentCoordinate.x - chunk.Key.Item1)
                + Mathf.Abs(currentCoordinate.z - chunk.Key.Item2);

            if (distance >= removeDistance)
            {
                chunk.Value.SetActive(false);
                chunkPool.Enqueue(chunk.Value);
                keysToRemove.Add(chunk.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            chunks.Remove(key);
        }
    }

    private void UpdateCreateChunk()
    {
        var playerCoordinate = ConvertWorldToChunkCoordinate(player.position);
        if (currentCoordinate == playerCoordinate)
            return;

        currentCoordinate = playerCoordinate;
        for (int i = currentCoordinate.x - 1; i <= currentCoordinate.x + 1; i++)
        {
            for (int j = currentCoordinate.z - 1; j <= currentCoordinate.z + 1; j++)
            {
                if (!chunks.ContainsKey((i, j)))
                    CreateChunk((i, j));
            }
        }
    }

    private void CreateChunk((int x, int z) coordinate)
    {
        if (chunks.ContainsKey(coordinate))
            return;

        var chunk = chunkPool.Dequeue();
        chunks[coordinate] = chunk;
        chunk.transform.position = new Vector3(coordinate.x * chunkWidth, 0, coordinate.z * chunkHeight);
        chunk.SetActive(true);
    }

    public (int, int) ConvertWorldToChunkCoordinate(Vector3 pos)
    {
        // chunk는 피벗이 중앙에 있다고 가정
        int x = Mathf.FloorToInt((pos.x + chunkWidth / 2f) / chunkWidth);
        int z = Mathf.FloorToInt((pos.z + chunkHeight / 2f) / chunkHeight);
        return (x, z);
    }

    private void Init()
    {
        for (int i = 0; i < chunkPoolCount; i++)
        {
            var chunk = Instantiate(chunkPrefab, transform);
            chunk.SetActive(false);
            chunkPool.Enqueue(chunk);
        }

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                CreateChunk((i, j));
            }
        }
    }
}