using System.Collections.Generic;
using UnityEngine;

public static class DataTableManger
{
    private static readonly Dictionary<string, DataTable> tables =
        new Dictionary<string, DataTable>();

    static DataTableManger()
    {
        Init();
    }

    private static void Init()
    {
        foreach (var id in DataTableIds.StringTableIds)
        {
            var table = new StringTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach(var id in DataTableIds.SpawnTableIds)
        {
            var table = new SpawnTable();
            table.Load(id);
            tables.Add(id, table);
        }

        {
            var MonsterTable = new MonsterDataTable();
            MonsterTable.Load(DataTableIds.MonsterTableId);
            tables.Add(DataTableIds.MonsterTableId, MonsterTable);
        }

        {
            var table = new BossMonsterDataTable();
            table.Load(DataTableIds.BossMonsterTableId);
            tables.Add(DataTableIds.BossMonsterTableId, table);
        }

        {
            var table = new ActiveSkillTable();
            table.Load(DataTableIds.ActiveSkillTableId);
            tables.Add(DataTableIds.ActiveSkillTableId, table);
        }
    }

    public static StringTable StringTable
    {
        get
        {
            return Get<StringTable>(DataTableIds.String);
        }
    }

    public static SpawnTable SpawnTable
    {
        get
        {
            return Get<SpawnTable>(DataTableIds.Spawn);
        }
    }

    public static MonsterDataTable MonsterTable
    {
        get
        {
            return Get<MonsterDataTable>(DataTableIds.MonsterTableId);
        }
    }

    public static BossMonsterDataTable BossMonsterTable
    {
        get
        {
            return Get<BossMonsterDataTable>(DataTableIds.BossMonsterTableId);
        }
    }

    public static ActiveSkillTable ActiveSkillTable
    {
        get
        {
            return Get<ActiveSkillTable>(DataTableIds.ActiveSkillTableId);
        }
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return null;
        }
        return tables[id] as T;
    }
}
