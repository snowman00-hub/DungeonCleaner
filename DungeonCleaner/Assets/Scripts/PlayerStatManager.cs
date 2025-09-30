using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
    public static readonly string ReinforceText = "강화하기";
    public static readonly string MaxText = "MAX";

    // Atk, HP, Def, Speed
    private const int TypeCount = 4;

    public TextMeshProUGUI[] currentStatValueTexts;
    public TextMeshProUGUI[] reinforceTexts;
    public Button[] upgradeButtons;
    public TextMeshProUGUI[] needMoneyTexts;

    private int[] statUpgradeCounts = new int[TypeCount];
    private int[] upgradePrices = new int[TypeCount];

    private float[] statValues = new float[TypeCount];
    private float[] reinforceValues = { 3, 30, 3, 0.5f };

    public int AtkUpgradeCount
    {
        get { return statUpgradeCounts[0]; }
        set
        {
            statUpgradeCounts[0] = value;
            currentStatValueTexts[0].text = $"공격력 : {statValues[0]}";
        }
    }

    public int HpUpgradeCount
    {
        get { return statUpgradeCounts[1]; }
        set
        {
            statUpgradeCounts[1] = value;
            currentStatValueTexts[1].text = $"체력 : {statValues[1]}";
        }
    }

    public int DefUpgradeCount
    {
        get { return statUpgradeCounts[2]; }
        set
        {
            statUpgradeCounts[2] = value;
            currentStatValueTexts[2].text = $"방어력 : {statValues[2]}";
        }
    }

    public int SpeedUpgradeCount
    {
        get { return statUpgradeCounts[3]; }
        set
        {
            statUpgradeCounts[3] = value;
            currentStatValueTexts[3].text = $"이동속도 : {statValues[3]}";
        }
    }

    private void OnEnable()
    {
        if (!SaveLoadManager.Load())
        {
            SaveLoadManager.Save();
        }

        var data = SaveLoadManager.Data;
        statValues[0] = data.atk;
        statValues[1] = data.maxHP;
        statValues[2] = data.def;
        statValues[3] = data.speed;
        AtkUpgradeCount = data.atkUpgradeCount;
        HpUpgradeCount = data.hpUpgradeCount;
        DefUpgradeCount = data.defUpgradeCount;
        SpeedUpgradeCount = data.speedUpgradeCount;

        UpdateReinforceSlots();
    }

    private void OnDisable()
    {
        var data = SaveLoadManager.Data;
        data.atk = Mathf.FloorToInt(statValues[0]);
        data.maxHP = Mathf.FloorToInt(statValues[1]);
        data.def = Mathf.FloorToInt(statValues[2]);
        data.speed = statValues[3];
        data.atkUpgradeCount = AtkUpgradeCount;
        data.hpUpgradeCount = HpUpgradeCount;
        data.defUpgradeCount = DefUpgradeCount;
        data.speedUpgradeCount = SpeedUpgradeCount;
        data.gold = MainHomeManager.Instance.MyMoney;
        SaveLoadManager.Save();
    }

    private void UpdateReinforceSlots()
    {
        var money = MainHomeManager.Instance.MyMoney;
        for (int i = 0; i < TypeCount; i++)
        {
            if (statUpgradeCounts[i] == 5)
            {
                needMoneyTexts[i].text = "0";
                reinforceTexts[i].text = statValues[i].ToString();
                var buttonText = upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = MaxText;
                upgradeButtons[i].interactable = false;
            }
            else
            {
                upgradePrices[i] = 1000 + 500 * statUpgradeCounts[i];
                needMoneyTexts[i].text = upgradePrices[i].ToString();
                reinforceTexts[i].text = $"{statValues[i]} + {reinforceValues[i]}";
                upgradeButtons[i].interactable = (money >= upgradePrices[i]);
                var buttonText = upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = ReinforceText;
            }
        }
    }

    public void Upgrade(int index)
    {
        if (statUpgradeCounts[index] == 5)
            return;

        MainHomeManager.Instance.MyMoney -= upgradePrices[index];
        statValues[index] += reinforceValues[index];

        switch (index)
        {
            case 0:
                AtkUpgradeCount++;
                break;
            case 1:
                HpUpgradeCount++;
                break;
            case 2:
                DefUpgradeCount++;
                break;
            case 3:
                SpeedUpgradeCount++;
                break;
        }

        UpdateReinforceSlots();
    }
}
