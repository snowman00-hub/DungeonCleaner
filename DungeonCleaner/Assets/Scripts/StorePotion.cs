using UnityEngine;
using UnityEngine.UI;

public enum PotionType
{
    atkPotion,
    expPotion,
    powerPotion,
}

public class StorePotion : MonoBehaviour
{
    public PotionType potionType;
    public int price;
    public int value;
    public GameObject outOfMoneyMessage;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TryBuyPotion);
    }

    private void TryBuyPotion()
    {
        if(StageInfoManager.Instance.Money >= price)
        {
            Player.Instance.UsePotion(this);
            StageInfoManager.Instance.Money -= price;
            StageInfoManager.Instance.CloseStore();
        }
        else
        {
            outOfMoneyMessage.SetActive(true);
        }
    }
}