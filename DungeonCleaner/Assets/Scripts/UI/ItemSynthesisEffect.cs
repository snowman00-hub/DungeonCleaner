using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemSynthesisEffect : MonoBehaviour
{
    public float synthesisTime = 1f;

    public ItemSlot synthesisSlot;
    public GameObject okButton;
    public GameObject successText;
    public GameObject failureText;

    public EquipItemData ingredientData;
    public EquipItemData resultData;

    public Slider synthesisSlider;

    public void StartEffect(EquipItemData ingredient, EquipItemData result, bool isSuccess)
    {
        successText.gameObject.SetActive(false);
        failureText.gameObject.SetActive(false);
        synthesisSlider.gameObject.SetActive(true);
        gameObject.SetActive(true);
        synthesisSlot.gameObject.SetActive(true);
        ingredientData = ingredient;
        resultData = result;
        StartCoroutine(ShuffleSlot(isSuccess));
        StartCoroutine(CoFillSlider());
    }

    private IEnumerator ShuffleSlot(bool isSuccess)
    {
        float elapsed = 0f;

        float delay = 0.2f;    // 시작 딜레이 (0.2초마다 교체)
        float minDelay = 0.05f; // 마지막에 도달할 최소 딜레이

        bool toggle = false;  

        while (elapsed < synthesisTime)
        {
            synthesisSlot.itemData = toggle ? ingredientData : resultData;
            synthesisSlot.UpdateItemSlotUI();
            toggle = !toggle;

            float t = elapsed / synthesisTime;
            float currentDelay = Mathf.Lerp(delay, minDelay, t);

            yield return new WaitForSeconds(currentDelay);

            elapsed += currentDelay;
        }

        if (isSuccess)
        {
            synthesisSlot.itemData = resultData;
            synthesisSlot.UpdateItemSlotUI();
            successText.gameObject.SetActive(true);
        }
        else
        {
            synthesisSlot.gameObject.SetActive(false);
            failureText.gameObject.SetActive(true);
        }

        okButton.SetActive(true);
        synthesisSlider.gameObject.SetActive(false);
    }

    private IEnumerator CoFillSlider()
    {
        synthesisSlider.value = 0f; 
        float elapsed = 0f;

        while (elapsed < synthesisTime)
        {
            elapsed += Time.deltaTime;
            synthesisSlider.value = Mathf.Clamp01(elapsed / synthesisTime);
            yield return null;
        }

        synthesisSlider.value = 1f;
    }
}