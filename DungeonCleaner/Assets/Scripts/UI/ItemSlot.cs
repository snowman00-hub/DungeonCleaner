using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Color DRankColor;
    public Color CRankColor;
    public Color BRankColor;
    public Color ARankColor;
    public Color SRankColor;

    public Image background;
    public Image itemImage;
    public TextMeshProUGUI reinforceCountText;

    [HideInInspector]
    public EquipItemData itemData;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            itemData = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.D);
            UpdateItemSlotUI();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            itemData = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.C);
            UpdateItemSlotUI();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            itemData = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.B);
            UpdateItemSlotUI();
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            itemData = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.A);
            UpdateItemSlotUI();
        }
    }

    private void UpdateItemSlotUI()
    {
        background.color = itemData.EQUIPMENT_GRADE switch
        {
            EquipItemRank.D => DRankColor,
            EquipItemRank.C => CRankColor,
            EquipItemRank.B => BRankColor,
            EquipItemRank.A => ARankColor,
            EquipItemRank.S => SRankColor,
        };

        itemImage.sprite = EquipItemImageManager.Instance.GetSprite(itemData.EQ_IMAGE_FILE_NAME);
        reinforceCountText.text = $"+ {itemData.REINFORCE_LEVEL}";
    }
}