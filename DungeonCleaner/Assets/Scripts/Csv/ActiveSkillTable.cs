using System.Collections.Generic;
using UnityEngine;

public enum ActiveSkillCategory
{
    Normal = 1,
    Evolution = 2,
}

public enum ActiveSkillType
{
    DirectProjectile = 1,
    PersistentProjectile = 2,
    SelfAura = 3,
    AreaAura = 4,
}

public class ActiveSkillData
{
    public string SKILL_ID { get; set; }
    public string SKILL_NAME { get; set; }
    public ActiveSkillCategory SKILL_CATEGORY { get; set; }
    public float SKILL_COOLTIME { get; set; }
    public float SKILL_DURATION { get; set; }
    public ActiveSkillType SKILL_ATTRIBUTE { get; set; }
    public bool USE_TICK { get; set; }
    public float TICK_INTERVAL { get; set; }
    public int PROJECTILE_COUNT { get; set; }
    public int DAMAGE { get; set; }
    public float SKILL_RADIAL { get; set; }
    public int CURRENT_LEVEL { get; set; }
    public float SKILL_SPEED { get; set; }
    public string DESCRIPTION { get; set; }
}

public class ActiveSkillTable : DataTable
{
    private readonly Dictionary<string, ActiveSkillData> dictionary = new Dictionary<string, ActiveSkillData>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<ActiveSkillData>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.SKILL_ID))
            {
                dictionary.Add(item.SKILL_ID, item);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {item.SKILL_ID}");
            }
        }
    }

    public ActiveSkillData Get(string id)
    {
        return dictionary[id];
    }
}
