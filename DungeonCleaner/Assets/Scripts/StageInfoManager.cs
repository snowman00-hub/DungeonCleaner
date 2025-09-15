using UnityEngine;

public class StageInfoManager : MonoBehaviour
{
    public static StageInfoManager Instance { get; private set; }

    [SerializeField]
    private StageInfoUI stageInfoUI;

    private int currentSeconds;
    private int money;
    private int killCount;
    public int currentXP;
    public int requiredXP;
    public int level;

    public float gameTimer;

    public int CurrentSeconds
    {
        get { return currentSeconds; }
        set
        {
            currentSeconds = value;
            stageInfoUI.SetTimeText(currentSeconds);
        }
    }

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

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameTimer += 60f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameTimer -= 60f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameTimer += 10f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameTimer -= 10f;
        }
#endif

        gameTimer += Time.deltaTime;
        int seconds = Mathf.FloorToInt(gameTimer);

        if (seconds != CurrentSeconds)
        {
            CurrentSeconds = seconds;
        }
    }
}
