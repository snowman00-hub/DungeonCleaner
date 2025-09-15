using System.Collections.Generic;
using UnityEngine;

public enum PickUpType
{
    smallExp = 1,
    mediumExp = 2,
    largeExp = 3,
    meat,
}

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager Instance { get; private set; }

    public List<GameObject> pickUpPrefabs;

    private Dictionary<PickUpType, Queue<PickUp>> pickUpPools = new Dictionary<PickUpType, Queue<PickUp>>();

    private int addPoolCount = 100;

    private void Awake()
    {
        Instance = this;
        foreach (var pickup in pickUpPrefabs)
        {
            var type = pickup.GetComponent<PickUp>().type;
            pickUpPools[type] = new Queue<PickUp>();

            for (int i = 0; i < addPoolCount; i++)
            {
                var go = Instantiate(pickup, transform);
                var pick_up = go.GetComponent<PickUp>();
                go.SetActive(false);

                pick_up.OnUsed += () =>
                {
                    go.SetActive(false);
                    pickUpPools[type].Enqueue(pick_up);
                };

                pickUpPools[type].Enqueue(pick_up);
            }
        }
    }

    public void CreatePickUp(PickUpType type, Vector3 position)
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

        go.gameObject.transform.position = position;
        go.gameObject.SetActive(true);
    }
}
