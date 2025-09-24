using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public void OneDrawItem()
    {
        if (MainHomeManager.Instance.MyJewel < 1)
            return;

        var data = DataTableManger.EquipItemTable.GetRandomItemWithChance();
        SaveLoadManager.Data.inventoryItemList.Add(data);

        MainHomeManager.Instance.MyJewel -= 1;
        SaveLoadManager.Data.jewel = MainHomeManager.Instance.MyJewel;
        SaveLoadManager.Save();
    }

    public void TenDrawItem()
    {
        if (MainHomeManager.Instance.MyJewel < 8)
            return;

        for (int i = 0; i < 10; i++)
        {
            var data = DataTableManger.EquipItemTable.GetRandomItemWithChance();
            SaveLoadManager.Data.inventoryItemList.Add(data);
        }

        MainHomeManager.Instance.MyJewel -= 8;
        SaveLoadManager.Data.jewel = MainHomeManager.Instance.MyJewel;
        SaveLoadManager.Save();
    }
}