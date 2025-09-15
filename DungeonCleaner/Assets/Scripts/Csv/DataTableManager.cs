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

        var normalMonsterTable = new NormalMonsterTable();
        normalMonsterTable.Load(DataTableIds.NormalMonsterTableId);
        tables.Add(DataTableIds.NormalMonsterTableId, normalMonsterTable);
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

    public static NormalMonsterTable NormalMonsterTable
    {
        get
        {
            return Get<NormalMonsterTable>(DataTableIds.NormalMonsterTableId);
        }
    }

    //public static ItemTable ItemTable
    //{
    //    get
    //    {
    //        return Get<ItemTable>(DataTableIds.Item);
    //    }
    //}

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
