using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StageInfoManager : MonoBehaviour
{
    public static StageInfoManager Instance { get; private set; }

    public GameObject skillChoiceWindow;

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
            currentSeconds = Mathf.Clamp(value, 0, 600);
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
            OpenSkillChoice();
        }
    }

    public int Money
    {
        get { return money; }
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

            if (currentXP >= requiredXP)
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
            gameTimer = Mathf.Clamp(gameTimer + 60f, 0, 600);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameTimer = Mathf.Clamp(gameTimer - 60f, 0, 600);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameTimer = Mathf.Clamp(gameTimer + 10f, 0, 600);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameTimer = Mathf.Clamp(gameTimer - 10f, 0, 600);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Level++;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CurrentXP += 20;
        }
#endif

        gameTimer += Time.deltaTime;
        int seconds = Mathf.FloorToInt(gameTimer);

        if (seconds != CurrentSeconds)
        {
            CurrentSeconds = seconds;
        }
    }

    public void OpenSkillChoice()
    {
        skillChoiceWindow.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseSkillChoice()
    {
        skillChoiceWindow.SetActive(false);
        Time.timeScale = 1f;
    }
}
