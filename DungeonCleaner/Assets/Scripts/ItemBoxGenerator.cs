using System.Collections.Generic;
using UnityEngine;

public class ItemBoxGenerator : MonoBehaviour
{
    public GameObject ItemBox;

    public float generateInterval = 5f;
    private float timer;

    public float minGenerateRadius = 25f;
    public float maxGenerateRadius = 30f;
    
    private Queue<GameObject> itemBoxPool = new Queue<GameObject>();
    private Transform target;

    private void Awake()
    {
        CreatePool(100);
    }

    private void Start()
    {
        target = Player.Instance.transform;
    }

    private void Update()
    {
        if(timer + generateInterval < Time.time)
        {
            timer = Time.time;
            if (itemBoxPool.Count == 0)
                CreatePool(50);

            var box = itemBoxPool.Dequeue();
            var generatePos = GetRandomPositionInRing3D(target.position, minGenerateRadius, maxGenerateRadius);
            box.transform.position = generatePos;
            box.SetActive(true);
        }
    }

    private void CreatePool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var box = Instantiate(ItemBox, transform);
            var itemBox = box.GetComponent<ItemBox>();
            itemBox.OnDestroy += () =>
            {
                box.SetActive(false);
                itemBoxPool.Enqueue(box);
            };

            box.SetActive(false);
            itemBoxPool.Enqueue(box);
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

    public void CreateNearByItemBox()
    {
        if (itemBoxPool.Count == 0)
            CreatePool(50);

        var box = itemBoxPool.Dequeue();
        var generatePos = GetRandomPositionInRing3D(target.position, 5f, 10f);
        box.transform.position = generatePos;
        box.SetActive(true);
    }
}
