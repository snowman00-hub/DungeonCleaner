using System.Collections.Generic;
using UnityEngine;

public class GachaResultWindow : MonoBehaviour
{
    public ItemSlot slotPrefab;
    public Transform gachaList;
    public float appearInterval = 0.4f;

    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    public void DisPlayResult(EquipItemData[] itemList)
    {
        gameObject.SetActive(true);
        for(int i=0;i<itemList.Length;i++)
        {

        }
    }

    private void OnDisable()
    {
        foreach (var slot in itemSlots)
        {
            Destroy(slot.gameObject);
        }
        itemSlots.Clear();
    }
}
