using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    MiniBoss = 1,
    Boss = 2,
}

public class BossMonsterData
{
    public EnemyName BMON_ID { get; set; }
    public string BMON_NAME { get; set; }
    public BossType BMON_TYPE { get; set; }
    public EnemyAttackType ATK_TYPE { get; set; }
    public int ATK { get; set; }
    public int MAXHP { get; set; }
    public float MOVE_SPEED { get; set; }
    public int STAGE { get; set; }
    public int PROJECTILE_COUNT { get; set; }
    public float PROJECTILE_RANGE { get; set; }
    public float PROJECTILE_COOLTIME { get; set; }
    public float PROJECTILE_MOVE_SPEED { get; set; }
    public PickUpType? DROP_ITEM1 { get; set; }
    public float? DROP_ITEM_VALUE1 { get; set; }
    public PickUpType? DROP_ITEM2 { get; set; }
    public float? DROP_ITEM_VALUE2 { get; set; }
    public PickUpType? DROP_ITEM3 { get; set; }
    public float? DROP_ITEM_VALUE3 { get; set; }
    public PickUpType DROP_EXP { get; set; }
    public float DROP_PER { get; set; }
}

public class BossMonsterDataTable : DataTable
{
    private readonly Dictionary<EnemyName, BossMonsterData> dictionary = new Dictionary<EnemyName, BossMonsterData>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<BossMonsterData>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.BMON_ID))
            {
                dictionary.Add(item.BMON_ID, item);
            }
            else
            {
                Debug.LogError($"키 중복: {item.BMON_ID}");
            }
        }
    }

    public BossMonsterData Get(EnemyName id)
    {
        return dictionary[id];
    }
}
