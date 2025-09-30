using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemSynthesisWindow : MonoBehaviour
{
    public ItemSlot resultSlot;
    public ItemSlot ingredientSlotLeft; // itemData만 받기
    public ItemSlot ingredientSlotRight;

    public TextMeshProUGUI DescText;

    public ItemSynthesisEffect effectWindow;

    private ItemSlot leftTemp; // slot 참조 받기
    private ItemSlot rightTemp;
    private EquipSynthesisData synthesisData;

    public AudioClip synthesisClip;
    public AudioClip synthesisSuccessClip;
    public AudioClip synthesisFailClip;
    private AudioSource audioSource;

    private bool CanSynthesis = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        resultSlot.gameObject.SetActive(false);
        ingredientSlotLeft.gameObject.SetActive(false);
        ingredientSlotRight.gameObject.SetActive(false);

        resultSlot.itemData = null;
        ingredientSlotLeft.itemData = null;
        ingredientSlotRight.itemData = null;

        leftTemp = rightTemp = null;
        CanSynthesis = false;
    }

    public void SelectSlot(ItemSlot slot)
    {
        if (slot.itemData.EQUIPMENT_GRADE == EquipItemRank.S)
            return;

        if (leftTemp == null || leftTemp == slot)
        {
            leftTemp = slot;
        }
        else if (leftTemp.itemData.EQ_IMAGE_FILE_NAME != slot.itemData.EQ_IMAGE_FILE_NAME)
        {
            leftTemp = slot;
            rightTemp = null;
        }
        else
        {
            rightTemp = slot;
            CanSynthesis = true;
        }

        var baseId = new string(slot.itemData.EQUIPMENT_ID.Where(c => !char.IsDigit(c)).ToArray());
        synthesisData = DataTableManger.EquipSynthesisTable.Get(baseId);

        resultSlot.itemData = DataTableManger.EquipItemTable.Get(synthesisData.SYN_RESULT);
        ingredientSlotLeft.itemData = slot.itemData;
        ingredientSlotRight.itemData = slot.itemData;

        resultSlot.gameObject.SetActive(true);
        ingredientSlotLeft.gameObject.SetActive(true);
        ingredientSlotRight.gameObject.SetActive(true);

        resultSlot.UpdateItemSlotUI();
        ingredientSlotLeft.UpdateItemSlotUI();
        ingredientSlotRight.UpdateItemSlotUI();

        DescText.text = $"{resultSlot.itemData.EQUIPMENT_NAME}\n" +
            $"MAX {resultSlot.itemData.BASE_STAT.ToString().ToUpper()}\n" +
            $"{DataTableManger.EquipItemTable.Get(baseId + "5").BASE_STAT_VALUE} -> {DataTableManger.EquipItemTable.Get(synthesisData.SYN_RESULT + "5").BASE_STAT_VALUE}\n" +
            $"성공 확률 : {synthesisData.SUC_PER}%\n" +
            $"{synthesisData.SPENDGOLD}골드";

        var resultFader = resultSlot.gameObject.GetComponent<UIFader>();
        resultFader.StartFadeInOut();
        var rightFader = ingredientSlotRight.gameObject.GetComponent<UIFader>();
        
        if(rightTemp == null)
        {
            rightFader.StartFadeInOut();
        }
        else
        {
            rightFader.StopFadeInOut();
        }
    }

    public void Synthesis()
    {
        float chance = synthesisData.SUC_PER;
        int price = synthesisData.SPENDGOLD;

        if (!CanSynthesis || MainHomeManager.Instance.MyMoney < price)
        {
            MainHomeManager.Instance.ErrorSound();
            return;
        }

        MainHomeManager.Instance.MyMoney -= price;
        var rand = Random.Range(0, 100);
        StartCoroutine(CoSynthesis(rand < chance , price));
    }

    private IEnumerator CoSynthesis(bool isSuccess, int price)
    {
        audioSource.PlayOneShot(synthesisClip);
        effectWindow.StartEffect(leftTemp.itemData, resultSlot.itemData, isSuccess);
        yield return new WaitForSeconds(effectWindow.synthesisTime);
        
        if (isSuccess)
        {
            Inventory.Instance.MakeItemSlot(resultSlot.itemData);
            audioSource.PlayOneShot(synthesisSuccessClip);
        }
        else
        {
            MainHomeManager.Instance.MyMoney += price / 2;
            audioSource.PlayOneShot(synthesisFailClip);
        }

        Inventory.Instance.RemoveItemSlot(leftTemp);
        Inventory.Instance.RemoveItemSlot(rightTemp);

        leftTemp = null;
        rightTemp = null;

        resultSlot.gameObject.SetActive(false);
        ingredientSlotLeft.gameObject.SetActive(false);
        ingredientSlotRight.gameObject.SetActive(false);

        var resultFader = resultSlot.gameObject.GetComponent<UIFader>();
        resultFader.StopFadeInOut();
        DescText.text = string.Empty;
    }
}