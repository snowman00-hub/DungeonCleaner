using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterData
{
    public string MON_ID {  get; set; }
    public string MON_NAME { get; set; }
    public EnemyAttackType MON_ATK_TYPE { get; set; }
    public int ATK { get; set; }
    public int HP { get; set; }
    public float MOVE_SPEED { get; set; }
    public PickUpType DROP_EXP {  get; set; }
    public float DROP_PER {  get; set; }
    public float PROJECTILE_RANGE { get; set; }
    public float PROJECTILE_COOLTIME { get; set; }
    public float PROJECTILE_MOVE_SPEED {  get; set; }
}

public class NormalMonsterTable : DataTable
{
    private readonly Dictionary<string, NormalMonsterData> dictionary = new Dictionary<string, NormalMonsterData>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<NormalMonsterData>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.MON_ID))
            {
                dictionary.Add(item.MON_ID, item);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {item.MON_ID}");
            }
        }
    }

    public NormalMonsterData Get(string id)
    {
        return dictionary[id];
    }
}
