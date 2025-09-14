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
    public int level;

    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            stageInfoUI.SetLevelText(level);
        }
    }

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

    public int CurrentXP
    {
        get { return currentXP; }
        set
        {
            currentXP = value;

            if(currentXP >= requiredXP)
            {
                Level++;
                currentXP = 0;
            }

            stageInfoUI.SetExpSliderValue(0, requiredXP, currentXP);
        }
    }

    public void AddExp(int add)
    {
        CurrentXP += add;
    }

    private void Awake()
    {
        Instance = this;
    }
}
