using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum SortType
{
    Grade,
    Level,
    EquipType,
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public GameObject[] currentEquipmentSlots = new GameObject[4];
    public ItemSlot itemSlotPrefab;

    private List<EquipItemData> inventoryItemList;
    private List<ItemSlot> inventoryItemSlots = new List<ItemSlot>();

    private SortType sortType;

    public ItemInfoWindow itemInfoWindow;

    private void Awake()
    {
        Instance = this;

        foreach (var go in currentEquipmentSlots)
        {
            var slot = go.GetComponent<ItemSlot>();
            var button = go.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                itemInfoWindow.gameObject.SetActive(true);
                itemInfoWindow.SetWindowUI(slot);
            });
        }
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

        var equipItemList = new List<EquipItemData>(SaveLoadManager.Data.equipItemList);
        for(int i = 0; i < currentEquipmentSlots.Length; i++)
        {
            InitialEquip(equipItemList[i], i);
        }

        sortType = SortType.Grade;
        SortBySortType();
    }

    private void OnDisable()
    {
        SaveLoadManager.Data.inventoryItemList = inventoryItemSlots.Select(s => s.itemData).ToList();
        SaveLoadManager.Data.equipItemList = currentEquipmentSlots.Select(x => x.GetComponent<ItemSlot>().itemData).ToArray();

        foreach(var go in currentEquipmentSlots)
        {
            go.SetActive(false);
        }

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
        itemSlot.button.onClick.AddListener(() =>
        {
            itemInfoWindow.gameObject.SetActive(true);
            itemInfoWindow.SetWindowUI(itemSlot);
        });

        inventoryItemSlots.Add(itemSlot);
    }

    public void InitialEquip(EquipItemData itemData, int index)
    {
        if (itemData == null)
            return;

        var currentSlot = currentEquipmentSlots[index].GetComponent<ItemSlot>();
        currentEquipmentSlots[index].SetActive(true);
        currentSlot.itemData = itemData;
        currentSlot.isEquipped = true;
        currentSlot.UpdateItemSlotUI();
    }

    public void EquipItem(ItemSlot slot, int index)
    {
        if (slot.itemData == null)
            return;

        var currentSlot = currentEquipmentSlots[index].GetComponent<ItemSlot>();
        if (currentSlot.itemData != null)
        {
            UnEquipItem(index);
        }

        currentEquipmentSlots[index].SetActive(true);
        currentSlot.itemData = slot.itemData;
        currentSlot.isEquipped = true;
        currentSlot.UpdateItemSlotUI();

        var data = SaveLoadManager.Data;
        switch (slot.itemData.BASE_STAT)
        {
            case EquipStatType.Atk:
                data.atk += slot.itemData.BASE_STAT_VALUE;
                break;
            case EquipStatType.HP:
                data.maxHP += slot.itemData.BASE_STAT_VALUE;
                break;
            case EquipStatType.Def:
                data.def += slot.itemData.BASE_STAT_VALUE;
                break;
        }

        data.equipItemList = currentEquipmentSlots.Select(x=> x.GetComponent<ItemSlot>().itemData).ToArray();
        inventoryItemSlots.Remove(slot);
        Destroy(slot.gameObject);
        SortBySortType();
        SaveLoadManager.Save();
    }

    public void UnEquipItem(int index)
    {
        var slot = currentEquipmentSlots[index].GetComponent<ItemSlot>();
        var data = slot.itemData;
        switch (slot.itemData.BASE_STAT)
        {
            case EquipStatType.Atk:
                SaveLoadManager.Data.atk -= slot.itemData.BASE_STAT_VALUE;
                break;
            case EquipStatType.HP:
                SaveLoadManager.Data.maxHP -= slot.itemData.BASE_STAT_VALUE;
                break;
            case EquipStatType.Def:
                SaveLoadManager.Data.def -= slot.itemData.BASE_STAT_VALUE;
                break;
        }

        slot.isEquipped = false;
        slot.itemData = null;
        currentEquipmentSlots[index].SetActive(false);        
        
        MakeItemSlot(data);
        SortBySortType();

        SaveLoadManager.Data.equipItemList = currentEquipmentSlots.Select(x => x.GetComponent<ItemSlot>().itemData).ToArray();
        SaveLoadManager.Data.inventoryItemList = inventoryItemSlots.Select(s => s.itemData).ToList();
        SaveLoadManager.Save();
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
