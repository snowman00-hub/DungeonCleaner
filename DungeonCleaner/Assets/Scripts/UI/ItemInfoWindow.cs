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

    public Button equipButton;
    public Button unEquipButton;

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

        if (slot.isEquipped)
        {
            equipButton.gameObject.SetActive(false);
            unEquipButton.gameObject.SetActive(true);
        }
        else
        {
            equipButton.gameObject.SetActive(true);
            unEquipButton.gameObject.SetActive(false);
        }
    }

    public void EquipCurrentItem()
    {
        int index = (int)currentSlot.itemData.EQUIPMENT_TYPE - 1;
        Inventory.Instance.EquipItem(currentSlot, index);
    }

    public void UnEquipCurrentItem()
    {        
        int index = (int)currentSlot.itemData.EQUIPMENT_TYPE - 1;
        Inventory.Instance.UnEquipItem(index);
    }
}
