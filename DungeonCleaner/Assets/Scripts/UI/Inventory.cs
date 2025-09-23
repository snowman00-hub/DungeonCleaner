using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SortType
{
    Grade,
    Level,
    EquipType,
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public ItemSlot[] currentEquipmentSlots = new ItemSlot[4];
    public ItemSlot itemSlotPrefab;

    private List<EquipItemData> inventoryItemList;
    private List<ItemSlot> inventoryItemSlots = new List<ItemSlot>();

    private SortType sortType;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (!SaveLoadManager.Load())
        {
            SaveLoadManager.Save();
        }

        inventoryItemList = new List<EquipItemData>(SaveLoadManager.Data.inventoryItemList);
        foreach (var item in inventoryItemList)
        {
            MakeItemSlot(item);
        }

        sortType = SortType.Grade;
        SortBySortType();
    }

    private void OnDisable()
    {
        SaveLoadManager.Data.inventoryItemList = inventoryItemSlots.Select(s => s.itemData).ToList();

        foreach (var slot in inventoryItemSlots)
        {
            Destroy(slot.gameObject);
        }
        inventoryItemSlots.Clear();
        SaveLoadManager.Save();
    }

    public void MakeItemSlot(EquipItemData data)
    {
        var itemSlot = Instantiate(itemSlotPrefab, transform);
        itemSlot.itemData = data;
        itemSlot.UpdateItemSlotUI();

        inventoryItemSlots.Add(itemSlot);
    }

    public void EquipItem(ItemSlot slot)
    {
        int equipIndex = (int)slot.itemData.EQUIPMENT_TYPE - 1;

        var currentSlotData = currentEquipmentSlots[equipIndex].itemData;
        currentEquipmentSlots[equipIndex].itemData = slot.itemData;
        currentEquipmentSlots[equipIndex].gameObject.SetActive(true);
        currentEquipmentSlots[equipIndex].UpdateItemSlotUI();

        if(currentSlotData != null)
        {

        }
    }

    public void SortItemSlotsByGrade()
    {
        sortType = SortType.Grade;

        var children = transform.Cast<Transform>()
                     .OrderByDescending(t => t.GetComponent<ItemSlot>().itemData.EQUIPMENT_GRADE)
                     .ToList();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    public void SortItemSlotsByType()
    {
        sortType = SortType.EquipType;

        var children = transform.Cast<Transform>()
                     .OrderBy(t => t.GetComponent<ItemSlot>().itemData.EQUIPMENT_TYPE)
                     .ToList();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    public void SortItemSlotByLevel()
    {
        sortType= SortType.Level;

        var children = transform.Cast<Transform>()
                     .OrderBy(t => t.GetComponent<ItemSlot>().itemData.REINFORCE_LEVEL)
                     .ToList();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    public void SortBySortType()
    {
        switch (sortType)
        {
            case SortType.Grade:
                SortItemSlotsByGrade();
                break;
            case SortType.Level:
                SortItemSlotByLevel();
                break;
            case SortType.EquipType:
                SortItemSlotsByType();
                break;
        }
    }

    // 테스트 코드
    public void GetRandomSlots()
    {
        var data = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.D);
        MakeItemSlot(data);
        var data1 = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.C);
        MakeItemSlot(data1);
        var data2 = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.B);
        MakeItemSlot(data2);
        var data3 = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.A);
        MakeItemSlot(data3);
        var data4 = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.S);
        MakeItemSlot(data4);
        SortBySortType();
    }
}
