using System.Collections.Generic;
using UnityEngine;

public class MonsterData
{
    public EnemyName MON_ID {  get; set; }
    public string MON_NAME { get; set; }
    public EnemyAttackType MON_ATK_TYPE { get; set; }
    public int ATK { get; set; }
    public int MAXHP { get; set; }
    public float MOVE_SPEED { get; set; }
    public PickUpType DROP_EXP {  get; set; }
    public float DROP_PER {  get; set; }
    public float PROJECTILE_RANGE { get; set; }
    public float PROJECTILE_COOLTIME { get; set; }
    public float PROJECTILE_MOVE_SPEED {  get; set; }
}

public class MonsterDataTable : DataTable
{
    private readonly Dictionary<EnemyName, MonsterData> dictionary = new Dictionary<EnemyName, MonsterData>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<MonsterData>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.MON_ID))
            {
                dictionary.Add(item.MON_ID, item);
            }
            else
            {
                Debug.LogError($"키 중복: {item.MON_ID}");
            }
        }
    }

    public MonsterData Get(EnemyName id)
    {
        return dictionary[id];
    }
}
