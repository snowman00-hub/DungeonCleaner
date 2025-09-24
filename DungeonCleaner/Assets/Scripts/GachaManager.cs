using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public void OneDrawItem()
    {
        var data = DataTableManger.EquipItemTable.GetRandomItemWithChance();
        SaveLoadManager.Data.inventoryItemList.Add(data);
        SaveLoadManager.Save();
    }

    public void TenDrawItem()
    {
        for(int i = 0; i < 10; i++)
        {
            var data = DataTableManger.EquipItemTable.GetRandomItemWithChance();
            SaveLoadManager.Data.inventoryItemList.Add(data);
        }
        SaveLoadManager.Save();
    }
}