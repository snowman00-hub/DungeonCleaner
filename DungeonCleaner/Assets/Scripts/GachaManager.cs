using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public GachaChestEffect chestEffect;
    public GachaResultWindow resultWindow;

    public Image flashImage;
    public float flashTime = 0.5f;

    private bool isPicking = false;

    public void OneDrawItem()
    {
        if (isPicking || MainHomeManager.Instance.MyJewel < 1)
            return;

        var list = new EquipItemData[1];
        var data = DataTableManger.EquipItemTable.GetRandomItemWithChance();
        SaveLoadManager.Data.inventoryItemList.Add(data);
        list[0] = data;

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
            var data = DataTableManger.EquipItemTable.GetRandomItemWithChance();
            SaveLoadManager.Data.inventoryItemList.Add(data);
        }

        MainHomeManager.Instance.MyJewel -= 8;
        SaveLoadManager.Data.jewel = MainHomeManager.Instance.MyJewel;
        SaveLoadManager.Save();

        StartCoroutine(CoGachaEffect(list));
    }

    private IEnumerator CoGachaEffect(EquipItemData[] itemList)
    {
        isPicking = true;
        chestEffect.StartEffect();
        yield return new WaitForSeconds(chestEffect.duration);
        StartCoroutine(CoFlash());
        yield return new WaitForSeconds(flashTime);
        resultWindow.DisPlayResult(itemList);
        yield return new WaitForSeconds(resultWindow.appearInterval * itemList.Length);
        isPicking = false;
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