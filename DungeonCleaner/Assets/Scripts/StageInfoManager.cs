using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInfoManager : MonoBehaviour
{
    public static StageInfoManager Instance { get; private set; }

    public GameObject skillChoiceWindow;
    public GameObject BossWall;

    [SerializeField]
    private StageInfoUI stageInfoUI;

    [SerializeField]
    private GameObject defeatWindow;
    public TextMeshProUGUI defeatTimeText;
    public TextMeshProUGUI defeatKillCount;

    [SerializeField]
    private GameObject victoryWindow;
    public TextMeshProUGUI victoryKillCount;

    public GameObject storeWindow;
    public GameObject pauseWindow;

    public GameObject bossHpBar;

    public StageReward stageReward;
    public TextMeshProUGUI victoryGoldRewardText;
    public TextMeshProUGUI victoryJewelRewardText;
    public TextMeshProUGUI defeatGoldRewardText;

    private int currentSeconds;
    private int money;
    private int killCount;
    public int currentXP;
    public int requiredXP;
    public int level;
        
    public float gameTimer;

    public int baseXP = 10;
    public float expUpRate = 1.2f;

    private bool IsExistWall = false;
    private float defaultTimeScale = 1f;

    public int CurrentSeconds
    {
        get { return currentSeconds; }
        set
        {
            currentSeconds = Mathf.Clamp(value, 0, 600);
            stageInfoUI.SetTimeText(currentSeconds);

            if(currentSeconds == 235 ||  currentSeconds == 475)
            {
                stageInfoUI.StartWarningMessage();
                AudioManager.Instance.BossWarning();
            }

            if(currentSeconds == 595)
            {
                stageInfoUI.StartFinalBossWarningMessage();
                AudioManager.Instance.FinalBossWarning();
            }

            if (currentSeconds == 600 && !IsExistWall)
            {
                DisappearEnemys();
                IsExistWall = true;
                Instantiate(BossWall, Player.Instance.transform.position, Quaternion.identity);
                bossHpBar.SetActive(true);
            }

            if(currentSeconds == 180 || currentSeconds == 360 || currentSeconds == 540)
            {
                OpenStore();
            }
        }
    }

    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            requiredXP = Mathf.FloorToInt(baseXP * Level * expUpRate);
            stageInfoUI.SetLevelText(level);
            OpenSkillChoice();
            AudioManager.Instance.LevelUp();
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
            LevelUp();
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
        Time.timeScale = defaultTimeScale;
    }

    public void Defeat()
    {
        Time.timeScale = 0f;
        defeatWindow.SetActive(true);
        defeatKillCount.text = stageInfoUI.killCountText.text;
        defeatTimeText.text = stageInfoUI.timeText.text;

        defeatGoldRewardText.text = $"x{money}";
        SaveLoadManager.Data.gold += money;
        SaveLoadManager.Save();

        AudioManager.Instance.GameOver();
    }

    public void Victory()
    {
        StartCoroutine(CoVictory());
    }

    private IEnumerator CoVictory()
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0f;
        victoryWindow.SetActive(true);
        victoryKillCount.text = stageInfoUI.killCountText.text;

        var goldreward = stageReward.goldReward + money;
        victoryGoldRewardText.text = $"x{goldreward}";
        victoryJewelRewardText.text = $"x{stageReward.jewelReward}";
        SaveLoadManager.Data.gold += goldreward;
        SaveLoadManager.Data.jewel += stageReward.jewelReward;
        SaveLoadManager.Save();
        AudioManager.Instance.Victory();
    }

    public void StartBombFlash()
    {
        stageInfoUI.StartBombFlashEffect();
    }

    public void OpenStore()
    {
        Time.timeScale = 0f;
        storeWindow.SetActive(true);
        AudioManager.Instance.MerchantCome();
    }

    public void CloseStore()
    {
        Time.timeScale = defaultTimeScale;
        storeWindow.SetActive(false);
    }

    public void OpenPauseWindow()
    {
        Time.timeScale = 0f;
        pauseWindow.SetActive(true);
    }

    public void ClosePauseWindow()
    {
        Time.timeScale = defaultTimeScale;
        pauseWindow.SetActive(false);
    }

    public void GoToMainHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    public void SaveReward()
    {
        SaveLoadManager.Data.gold += money;
        SaveLoadManager.Save();
    }

    private void DisappearEnemys()
    {
        var enemys = GameObject.FindGameObjectsWithTag(Tag.Enemy);
        foreach (var enemy in enemys)
        {
            enemy.SetActive(false);
        }
    }

    // 빌드 테스트 코드
    public void AddOneMinute()
    {
        gameTimer = Mathf.Clamp(gameTimer + 60f, 0, 600);
    }
    public void LevelUp()
    {
        if (Time.timeScale == 0f)
            return;

        Level++;
    }
    public void RestartScene()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void GoldGet()
    {
        Money += 5000;
    }
    public void MeetBoss()
    {
        gameTimer = 595f;
    }
    public void SetTimeScale(float f)
    {
        Time.timeScale = f;
        defaultTimeScale = f;
    }
    //        
}
