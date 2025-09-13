using UnityEngine;

public class StageInfoManager : MonoBehaviour
{
    public static StageInfoManager Instance { get; private set; }

    [SerializeField]
    private StageInfoUI stageInfoUI;

    public float currentTime;
    private int money;
    private int killCount;
    public int currentXP;
    public int requiredXP;

    public int Money
    {
        get {  return money; }
        set
        {
            stageInfoUI.SetGoldText(value);
            money = value;
        }
    }

    public int KillCount
    {
        get { return killCount; }
        set
        {
            stageInfoUI.SetKillCountText(value);
            killCount = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
}
