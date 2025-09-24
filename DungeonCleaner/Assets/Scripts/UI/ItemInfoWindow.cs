using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI reinforceCountText;
    [SerializeField]
    private TextMeshProUGUI itemDescText;
    [SerializeField]
    private TextMeshProUGUI statTypeText;
    [SerializeField]
    private TextMeshProUGUI statValueText;

    private ItemSlot currentSlot;

    public void SetWindowUI(ItemSlot slot)
    {
        currentSlot = slot;
        var data = slot.itemData;
        itemNameText.text = data.EQUIPMENT_NAME;
        background.color = slot.background.color;
        itemImage.sprite = slot.itemImage.sprite;
        reinforceCountText.text = slot.reinforceCountText.text;
        itemDescText.text = data.EQ_EXPLAIN;
        statTypeText.text = data.BASE_STAT.ToString();
        statValueText.text = data.BASE_STAT_VALUE.ToString();
    }

    public void EquipCurrentItem()
    {
        //Inventory.Instance.EquipItem()
    }
}
