using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public static BossHpBar Instance { get; private set; }

    public Slider hpBar;

    private void Awake()
    {
        Instance = this;
        hpBar = GetComponent<Slider>();
    }
}
