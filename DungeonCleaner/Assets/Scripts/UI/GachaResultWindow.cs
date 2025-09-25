using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaResultWindow : MonoBehaviour
{
    public ItemSlot slotPrefab;
    public Transform gachaList;
    public float appearInterval = 0.4f;

    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    public AudioClip appearClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void DisPlayResult(EquipItemData[] itemList)
    {
        gameObject.SetActive(true);
        StartCoroutine(CoAppearSlots(itemList));
    }

    private IEnumerator CoAppearSlots(EquipItemData[] itemList)
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            var slot = Instantiate(slotPrefab, gachaList);
            slot.itemData = itemList[i];
            slot.UpdateItemSlotUI();
            itemSlots.Add(slot);
            audioSource.PlayOneShot(appearClip);
            yield return new WaitForSeconds(appearInterval);
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
