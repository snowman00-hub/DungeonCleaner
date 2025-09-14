using System.Collections.Generic;
using UnityEngine;

public enum PickUpType
{
    smallExp,
    mediumExp,
    largeExp,
    meat,
}

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager Instance { get; private set; }

    public List<PickUp> pickUpPrefabs;

    private Dictionary<PickUpType, Queue<PickUp>> pickUpPools = new Dictionary<PickUpType, Queue<PickUp>>();

    private int addPoolCount = 100;

    private void Awake()
    {
        Instance = this;
        foreach(var pickup in pickUpPrefabs)
        {
            var type = pickup.type;
            pickUpPools[type] = new Queue<PickUp>();

            for (int i = 0; i < addPoolCount; i++)
            {
                var go = Instantiate(pickup, transform);
                go.gameObject.SetActive(false);

                go.OnUsed += () =>
                {
                    go.gameObject.SetActive(false);
                    pickUpPools[type].Enqueue(go);
                };

                pickUpPools[type].Enqueue(go);
            }
        }
    }

    public void CreatePickUp(PickUpType type, Vector3 position, int value)
    {
        var go = pickUpPools[type].Dequeue();

        if (pickUpPools[type].Count == 0)
        {
            for (int i = 0; i < addPoolCount; i++)
            {
                var temp = Instantiate(go, transform);
                temp.gameObject.SetActive(false);

                temp.OnUsed += () =>
                {
                    temp.gameObject.SetActive(false);
                    pickUpPools[type].Enqueue(temp);
                };

                pickUpPools[type].Enqueue(temp);
            }
        }

        go.value = value;
        go.gameObject.transform.position = position;
        go.gameObject.SetActive(true);
    }
}
