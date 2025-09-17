using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillData
{
    public string SKILL_ID { get; set; }
    public string SKILL_NAME { get; set; }
    public StatType AFFECT_ABILITY { get; set; }
    public float PASSIVE_VALUE { get; set; }
    public int CRAFT_CODE { get; set; }
    public string DESCRIPTION { get; set; }
    public int SKILL_LEVEL { get; set; }
}

public class PassiveSkillTable : DataTable
{
    private readonly Dictionary<string, PassiveSkillData> dictionary = new Dictionary<string, PassiveSkillData>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<PassiveSkillData>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.SKILL_ID))
            {
                dictionary.Add(item.SKILL_ID, item);
            }
            else
            {
                Debug.LogError($"키 중복: {item.SKILL_ID}");
            }
        }
    }

    public PassiveSkillData Get(string id)
    {
        return dictionary[id];
    }
}
