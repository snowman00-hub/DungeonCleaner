using System.Collections.Generic;
using UnityEngine;

public class InfiniteMapGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    public float chunkWidth = 30f;
    public float chunkHeight = 30f;

    private Transform playerPos;

    private const int chunkPoolCount = 50;
    private Queue<GameObject> chunkPool = new Queue<GameObject>();

    private List<(int, int)> coordinates = new List<(int, int)>();
    private (int, int) prevCoordinate = (0, 0);

    private void Start()
    {
        var player = GameObject.FindWithTag(Tag.Player);
        playerPos = player.transform;
        Init();
    }

    private void Update()
    {
        var currentCoordinate = GetPlayerChunkCoordinate();
        if (prevCoordinate == currentCoordinate)
            return;

        prevCoordinate = currentCoordinate;
        CheckAndCreateChunk(currentCoordinate);
    }

    private void CheckAndCreateChunk((int x, int z) coordinate)
    {
        for (int i = coordinate.x - 1; i <= coordinate.x + 1; i++)
        {
            for (int j = coordinate.z - 1; j <= coordinate.z + 1; j++)
            {
                if (coordinates.Contains((i, j)))
                    continue;

                CreateChunk((i, j));
            }
        }
    }

    private void CreateChunk((int x, int z) coordinate)
    {
        if (coordinates.Contains(coordinate))
            return;

        coordinates.Add(coordinate);
        var chunk = chunkPool.Dequeue();
        chunk.transform.position = new Vector3(coordinate.x * chunkWidth, 0, coordinate.z * chunkHeight);
        chunk.SetActive(true);
    }

    public (int, int) GetPlayerChunkCoordinate()
    {
        // chunk는 피벗이 중앙에 있다고 가정
        int x = Mathf.FloorToInt((playerPos.position.x + chunkWidth / 2f) / chunkWidth);
        int z = Mathf.FloorToInt((playerPos.position.z + chunkHeight / 2f) / chunkHeight);
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