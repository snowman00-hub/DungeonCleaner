using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public Color DRankColor;
    public Color CRankColor;
    public Color BRankColor;
    public Color ARankColor;
    public Color SRankColor;

    public GachaChestEffect chestEffect;
    public GachaResultWindow resultWindow;

    public Image flashImage;
    public float flashTime = 0.5f;

    private bool isPicking = false;

    public AudioClip chestShakeClip;
    public AudioClip chestOpenClip;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        isPicking = false;
        resultWindow.gameObject.SetActive(false);
    }

    public void OneDrawItem()
    {
        if (isPicking || MainHomeManager.Instance.MyJewel < 1)
            return;

        var list = new EquipItemData[1];
        var data = DataTableManger.EquipItemTable.GetRandomItemWithChance();
        SaveLoadManager.Data.inventoryItemList.Add(data);
        list[0] = data;
        SetFlashColor(data.EQUIPMENT_GRADE);

        MainHomeManager.Instance.MyJewel -= 1;
        SaveLoadManager.Data.jewel = MainHomeManager.Instance.MyJewel;
        SaveLoadManager.Save();

        StartCoroutine(CoGachaEffect(list));
    }

    public void TenDrawItem()
    {
        if (isPicking || MainHomeManager.Instance.MyJewel < 8)
            return;

        var list = new EquipItemData[10];
        for (int i = 0; i < 10; i++)
        {
            list[i] = DataTableManger.EquipItemTable.GetRandomItemWithChance();
        }

        var highestRank = list.Max(x => x.EQUIPMENT_GRADE);
        if(highestRank < EquipItemRank.B)
        {
            list[list.Length - 1] = DataTableManger.EquipItemTable.GetRandomItem(EquipItemRank.B);
            highestRank = EquipItemRank.B;
        }
        SetFlashColor(highestRank);

        for(int i = 0; i < list.Length; i++)
        {
            SaveLoadManager.Data.inventoryItemList.Add(list[i]);
        }

        MainHomeManager.Instance.MyJewel -= 8;
        SaveLoadManager.Data.jewel = MainHomeManager.Instance.MyJewel;
        SaveLoadManager.Save();

        StartCoroutine(CoGachaEffect(list));
    }

    private void SetFlashColor(EquipItemRank rank)
    {
        flashImage.color = rank switch
        {
            EquipItemRank.D => DRankColor,
            EquipItemRank.C => CRankColor,
            EquipItemRank.B => BRankColor,
            EquipItemRank.A => ARankColor,
            EquipItemRank.S => SRankColor,
            _ => DRankColor,
        };
    }

    private IEnumerator CoGachaEffect(EquipItemData[] itemList)
    {
        isPicking = true;
        resultWindow.gameObject.SetActive(false);
        chestEffect.StartEffect();
        audioSource.PlayOneShot(chestShakeClip);
        yield return new WaitForSeconds(chestEffect.duration);
        StartCoroutine(CoFlash());
        audioSource.PlayOneShot(chestOpenClip);
        yield return new WaitForSeconds(flashTime);
        resultWindow.DisPlayResult(itemList);
        isPicking = false;
        yield return new WaitForSeconds(resultWindow.appearInterval * itemList.Length);
    }

    private IEnumerator CoFlash()
    {
        yield return StartCoroutine(FadeAlpha(0f, 1f, flashTime/ 2f));
        yield return StartCoroutine(FadeAlpha(1f, 0f, flashTime / 2f));
    }

    private IEnumerator FadeAlpha(float from, float to, float time)
    {
        float t = 0f;
        Color c = flashImage.color;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            c.a = Mathf.Lerp(from, to, t);
            flashImage.color = c;
            yield return null;
        }
    }
}