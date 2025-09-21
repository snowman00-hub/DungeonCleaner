using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusSkill : MonoBehaviour
{
    private Button button;
    public TextMeshProUGUI text;

    public int moneyAmount = 1000;
    public int healAmount = 50;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void AddGold()
    {
        text.text = $"{moneyAmount} 골드";
        button.onClick.AddListener(() => StageInfoManager.Instance.Money += moneyAmount);
        button.onClick.AddListener(() => AudioManager.Instance.Click());
        button.onClick.AddListener(() => StageInfoManager.Instance.CloseSkillChoice());
    }

    public void Heal()
    {
        text.text = $"{healAmount} 회복";
        button.onClick.AddListener(() => Player.Instance.Heal(healAmount));
        button.onClick.AddListener(() => AudioManager.Instance.Click());
        button.onClick.AddListener(() => StageInfoManager.Instance.CloseSkillChoice());
    }
}
