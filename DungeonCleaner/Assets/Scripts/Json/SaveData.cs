using System;
using System.Collections.Generic;

[Serializable]
public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

[Serializable]
public class SaveDataV1 : SaveData
{
    public int maxHP;
    public int atk;
    public float finalAttackMultiplier;
    public int def;
    public float finalDamageReduction;
    public float speed;
    public float activeSkillDurationMultiplier;
    public float recoveryPercent;
    public float pickUpRadius;

    public int gold;
    public int jewel;
    public int atkUpgradeCount;
    public int hpUpgradeCount;
    public int defUpgradeCount;
    public int speedUpgradeCount;

    public EquipItemData[] equipItemList = new EquipItemData[4];
    public List<EquipItemData> inventoryItemList = new List<EquipItemData>();

    public SaveDataV1()
    {
        Version = 1;
        maxHP = 200;
        atk = 20;
        finalAttackMultiplier = 1f;
        finalDamageReduction = 0f;
        def = 5;
        speed = 7;
        activeSkillDurationMultiplier = 1f;
        recoveryPercent = 0f;
        pickUpRadius = 2f;
        gold = 100000;
        jewel = 50;
    }

    public override SaveData VersionUp()
    {
        throw new NotImplementedException();
    }
}