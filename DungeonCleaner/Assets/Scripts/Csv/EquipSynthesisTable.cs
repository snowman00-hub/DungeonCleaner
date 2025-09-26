using System.Collections.Generic;
using UnityEngine;

public class EquipSynthesisData
{
    public string SYN_INGRED {  get; set; }
    public string SYN_RESULT {  get; set; }
    public int INGRED_ITEM {  get; set; }
    public int SPENDGOLD { get; set; }
    public float SUC_PER {  get; set; }
    public float FAIL_PER { get; set; }
    public int FAIL_REWARD { get; set; }
}

public class EquipSynthesisTable : DataTable
{
    private readonly Dictionary<string, EquipSynthesisData> dictionary = new Dictionary<string, EquipSynthesisData>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<EquipSynthesisData>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.SYN_INGRED))
            {
                dictionary.Add(item.SYN_INGRED, item);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {item.SYN_INGRED}");
            }
        }
    }

    public EquipSynthesisData Get(string id)
    {
        return dictionary[id];
    }
}