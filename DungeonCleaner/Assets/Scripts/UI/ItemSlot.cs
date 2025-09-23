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

    public void UpdateItemSlotUI()
    {
        background.color = itemData.EQUIPMENT_GRADE switch
        {
            EquipItemRank.D => DRankColor,
            EquipItemRank.C => CRankColor,
            EquipItemRank.B => BRankColor,
            EquipItemRank.A => ARankColor,
            EquipItemRank.S => SRankColor,
            _ => DRankColor,
        };

        itemImage.sprite = EquipItemImageManager.Instance.GetSprite(itemData.EQ_IMAGE_FILE_NAME);
        reinforceCountText.text = $"+ {itemData.REINFORCE_LEVEL}";
    }
}