using UnityEngine;

public enum Languages
{
    Korean,
}

public class StringIds
{
    public static readonly string Test = "Test";
}

public static class DataTableIds
{
    public static readonly string[] StringTableIds =
    {
        "StringTableKr",
    };

    public static readonly string[] SpawnTableIds =
    {
        "spawnTable1",
    };

    public static readonly string MonsterTableId = "MonsterTable";
    public static readonly string BossMonsterTableId = "BossMonsterTable";

    public static readonly string ActiveSkillTableId = "ActiveSkillTable";
    public static readonly string PassiveSkillTableId = "PassiveSkillTable";

    public static string String => StringTableIds[(int)Variables.Language];
    public static string Spawn => SpawnTableIds[Variables.CurrentStageNumber - 1];

    public static readonly string Item = "ItemTable";
}

public static class Variables
{
    public static Languages Language = Languages.Korean;
    public static int CurrentStageNumber = 1;
}

public class Tag
{
    public static readonly string Player = "Player";
    public static readonly string Enemy = "Enemy";
    public static readonly string Exp = "Exp";
    public static readonly string EnemyAttack = "EnemyAttack";
}

public class LayerName
{
    public static readonly string PickUp = "PickUp";
}