using System.Linq;
using TMPro;
using UnityEngine;

public class ItemReinforceWindow : MonoBehaviour
{
    public ItemSlot displaySlot;

    public GameObject explainPanel;
    public TextMeshProUGUI statTypeText;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI reinforceFeeText;

    private ItemSlot currentSlot;

    private AudioSource audioSource;
    public AudioClip reinforceClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        explainPanel.SetActive(false);
        displaySlot.gameObject.SetActive(false);
    }

    public void ReinforceItem()
    {
        if (currentSlot == null)
            return;
        
        if (MainHomeManager.Instance.MyMoney < currentSlot.itemData.REINFORCE_FEE)
            return;

        if (currentSlot.itemData.REINFORCE_LEVEL == 5)
            return;

        MainHomeManager.Instance.MyMoney -= currentSlot.itemData.REINFORCE_FEE;
        var baseId = new string(currentSlot.itemData.EQUIPMENT_ID.Where(c => !char.IsDigit(c)).ToArray());
        var nextLevelId = baseId + (currentSlot.itemData.REINFORCE_LEVEL + 1).ToString();
        var data = DataTableManger.EquipItemTable.Get(nextLevelId);

        currentSlot.itemData = data;
        DisPlayItemData(currentSlot);
        currentSlot.UpdateItemSlotUI();
        audioSource.PlayOneShot(reinforceClip);
    }

    public void DisPlayItemData(ItemSlot itemSlot)
    {
        currentSlot = itemSlot;
        var data = itemSlot.itemData;

        displaySlot.gameObject.SetActive(true);
        displaySlot.itemData = data;
        displaySlot.UpdateItemSlotUI();

        explainPanel.SetActive(true);

        statTypeText.text = data.BASE_STAT.ToString();
        if (data.REINFORCE_LEVEL == 5)
        {
            valueText.text = data.BASE_STAT_VALUE.ToString();
            reinforceFeeText.text = "MAX";
        }
        else
        {
            var baseId = new string(data.EQUIPMENT_ID.Where(c => !char.IsDigit(c)).ToArray());
            var nextLevelId = baseId + (data.REINFORCE_LEVEL + 1).ToString();
            var nextLevelValue = DataTableManger.EquipItemTable.Get(nextLevelId).BASE_STAT_VALUE;

            valueText.text = $"{data.BASE_STAT_VALUE} -> {nextLevelValue}";
            reinforceFeeText.text = data.REINFORCE_FEE.ToString();
        }
    }
}