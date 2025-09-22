using TMPro;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public TextMeshProUGUI currentAtkText;
    public TextMeshProUGUI currentHpText;
    public TextMeshProUGUI currentDefText;
    public TextMeshProUGUI currentSpeedText;

    private int atkUpgradeCount;
    private int hpUpgradeCount;
    private int defUpgradeCount;
    private int speedUpgradeCount;

    private int atk;
    private int hp;
    private int def;
    private float speed;

    public int AtkUpgradeCount
    {
        get { return atkUpgradeCount; }
        set
        {
            atkUpgradeCount = value;
            currentAtkText.text = $"공격력 : {atk}";
        }
    }

    public int HpUpgradeCount
    {
        get { return hpUpgradeCount; }
        set
        {
            hpUpgradeCount = value;
            currentHpText.text = $"체력 : {hp}";
        }
    }

    public int DefUpgradeCount
    {
        get { return defUpgradeCount; }
        set
        {
            defUpgradeCount = value;
            currentDefText.text = $"방어력 : {def}";
        }
    }

    public int SpeedUpgradeCount
    {
        get { return  speedUpgradeCount; }
        set
        {
            speedUpgradeCount = value;
            currentSpeedText.text = $"이동속도 : {speed}";
        }
    }

    private void OnEnable()
    {
        if (!SaveLoadManager.Load())
        {
            SaveLoadManager.Save();
        }

        var data = SaveLoadManager.Data;
        atk = data.atk;
        hp = data.maxHP;
        def = data.def;
        speed = data.speed;
        AtkUpgradeCount = data.atkUpgradeCount;
        HpUpgradeCount = data.hpUpgradeCount;
        DefUpgradeCount = data.defUpgradeCount;
        SpeedUpgradeCount = data.speedUpgradeCount;
    }

    private void OnDisable()
    {
        var data = SaveLoadManager.Data;
        data.atk = atk;
        data.maxHP = hp;
        data.def = def;
        data.speed = speed;
        data.atkUpgradeCount = atkUpgradeCount;
        data.hpUpgradeCount = hpUpgradeCount;
        data.defUpgradeCount = defUpgradeCount; 
        data.speedUpgradeCount = speedUpgradeCount;
        SaveLoadManager.Save();
    }
}
