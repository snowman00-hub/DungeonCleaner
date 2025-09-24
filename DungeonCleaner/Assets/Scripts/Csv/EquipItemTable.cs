using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EquipItemRank
{
    D = 1,
    C = 2,
    B = 3,
    A = 4,
    S = 5,
}

public enum EquipmentType
{
    Weapon = 1,
    Top = 2,
    Bottom = 3,
    Shoes = 4,
}

public enum EquipStatType
{
    Atk = 1,
    HP = 2,
    Def = 3,
}

public class EquipItemData
{
    public string EQUIPMENT_ID { get; set; }
    public string EQUIPMENT_NAME { get; set; }
    public EquipItemRank EQUIPMENT_GRADE { get; set; }
    public int REINFORCE_LEVEL { get; set; }
    public EquipmentType EQUIPMENT_TYPE { get; set; }
    public EquipStatType BASE_STAT { get; set; }
    public int BASE_STAT_VALUE { get; set; }
    public string EQ_EXPLAIN { get; set; }
    public string EQ_IMAGE_FILE_NAME { get; set; }
    public int REINFORCE_FEE { get; set; }
}

public class EquipItemTable : DataTable
{
    private readonly Dictionary<string, EquipItemData> dictionary = new Dictionary<string, EquipItemData>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<EquipItemData>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.EQUIPMENT_ID))
            {
                dictionary.Add(item.EQUIPMENT_ID, item);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {item.EQUIPMENT_ID}");
            }
        }
    }

    public EquipItemData Get(string id)
    {
        return dictionary[id];
    }

    public EquipItemData GetRandomItem(EquipItemRank rank)
    {
        var rankList = dictionary.Values.Where(x => x.EQUIPMENT_GRADE == rank).ToList();
        var list = rankList.Where(x => x.REINFORCE_LEVEL == 0).ToList();

        var rand = Random.Range(0, list.Count);
        return list[rand];
    }

    private float Schance = 2.0f;
    private float Achance = 8.0f;
    private float Bchance = 15.0f;
    private float Cchance = 25.0f;
    //private float Dchance = 50.0f;

    public EquipItemData GetRandomItemWithChance()
    {
        float rand = Random.Range(0f, 100f);
        if (rand <= Schance)
        {
            return GetRandomItem(EquipItemRank.S);
        }
        else if(rand <= Schance + Achance)
        {
            return GetRandomItem(EquipItemRank.A);
        }
        else if(rand <= Schance + Achance + Bchance)
        {
            return GetRandomItem(EquipItemRank.B);
        }
        else if(rand <= Schance + Achance + Bchance + Cchance)
        {
            return GetRandomItem(EquipItemRank.C);
        }
        else
        {
            return GetRandomItem(EquipItemRank.D);
        }
    }
}
