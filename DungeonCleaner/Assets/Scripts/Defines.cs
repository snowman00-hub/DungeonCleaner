using UnityEngine;
using static Unity.VisualScripting.Icons;
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
    public static string String => StringTableIds[(int)Variables.Language];

    public static readonly string Item = "ItemTable";
}

public static class Variables
{
    public static Languages Language = Languages.Korean;
}

public class Tag
{
    public static readonly string Player = "Player";
}