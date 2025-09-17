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
    public float pickUpRadius;

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        throw new NotImplementedException();
    }
}