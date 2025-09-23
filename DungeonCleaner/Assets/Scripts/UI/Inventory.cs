using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot itemSlotPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var data = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.D);
            MakeItemSlot(data);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var data = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.C);
            MakeItemSlot(data);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var data = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.B);
            MakeItemSlot(data);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            var data = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.A);
            MakeItemSlot(data);
        }
    }

    public void MakeItemSlot(EquipItemData data)
    {
        var itemSlot = Instantiate(itemSlotPrefab, transform);
        itemSlot.itemData = data;
        itemSlot.UpdateItemSlotUI();
    }
}
