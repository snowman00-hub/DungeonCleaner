using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class SpawnData
{
    public int SPAWN_ID { get; set; }
    public EnemyName MON_NAME { get; set; }
    public int START_TIME { get; set; }
    public int END_TIME { get; set; }
    public EnemyType MON_TYPE { get; set; }
    public int MON_COUNT { get; set; }
    public float INTERVAL { get; set; }
    public float WEIGHT { get; set; }
}

public class SpawnTable : DataTable
{
    private readonly Dictionary<int, SpawnData> dictionary = new Dictionary<int, SpawnData>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<SpawnData>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.SPAWN_ID))
            {
                dictionary.Add(item.SPAWN_ID, item);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {item.SPAWN_ID}");
            }
        }
    }

    public List<SpawnData> GetList()
    { 
        return dictionary.Values.ToList();
    }

    public SpawnData Get(int id)
    {
        return dictionary[id];
    }
}
