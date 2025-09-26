using System.Linq;
using TMPro;
using UnityEngine;

public class ItemSynthesisWindow : MonoBehaviour
{
    public ItemSlot resultSlot;
    public ItemSlot ingredientSlotLeft;
    public ItemSlot ingredientSlotRight;

    public TextMeshProUGUI DescText;

    private ItemSlot leftTemp;
    private ItemSlot rightTemp;
    private EquipSynthesisData synthesisData;

    public AudioClip synthesisClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        resultSlot.gameObject.SetActive(false);
        ingredientSlotLeft.gameObject.SetActive(false);
        ingredientSlotRight.gameObject.SetActive(false);

        resultSlot.itemData = null;
        ingredientSlotLeft.itemData = null;
        ingredientSlotRight.itemData = null;        
    }

    public void SelectSlot(ItemSlot slot)
    {
        if (slot.itemData.EQUIPMENT_GRADE == EquipItemRank.S)
            return;

        var baseId = new string(slot.itemData.EQUIPMENT_ID.Where(c => !char.IsDigit(c)).ToArray());
        synthesisData = DataTableManger.EquipSynthesisTable.Get(baseId);

        resultSlot.itemData = DataTableManger.EquipItemTable.Get(synthesisData.SYN_RESULT);
        ingredientSlotLeft.itemData = slot.itemData;
        ingredientSlotRight.itemData = slot.itemData;

        resultSlot.gameObject.SetActive(true);
        ingredientSlotLeft.gameObject.SetActive(true);
        ingredientSlotRight.gameObject.SetActive(true);

        resultSlot.UpdateItemSlotUI();
        ingredientSlotLeft.UpdateItemSlotUI();
        ingredientSlotRight.UpdateItemSlotUI();

        DescText.text = $"{resultSlot.itemData.EQUIPMENT_NAME}\n" +
            $"MAX {resultSlot.itemData.BASE_STAT.ToString().ToUpper()}\n" +
            $"{DataTableManger.EquipItemTable.Get(baseId + "5").BASE_STAT_VALUE} -> {DataTableManger.EquipItemTable.Get(synthesisData.SYN_RESULT + "5").BASE_STAT_VALUE}\n\n" +
            $"{synthesisData.FAIL_REWARD}골드";
    }

    public void Synthesis()
    {
        float chance = synthesisData.SUC_PER;
        int price = synthesisData.SPENDGOLD;
        var rand = Random.Range(0, 100);
        if(rand < chance)
        {
            // 성공
        }
        else
        {
            // 실패
        }
        Inventory.Instance.RemoveItemSlot(leftTemp);
        Inventory.Instance.RemoveItemSlot(rightTemp);
        audioSource.PlayOneShot(synthesisClip);
    }
}