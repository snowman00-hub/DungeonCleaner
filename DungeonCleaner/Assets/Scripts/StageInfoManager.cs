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
            }

            if(currentSeconds == 595)
            {
                stageInfoUI.StartFinalBossWarningMessage();
            }

            if (currentSeconds == 600 && !IsExistWall)
            {
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
                requiredXP = Mathf.FloorToInt(baseXP * Level * expUpRate);                
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
            stageInfoUI.StartWarningMessage();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            stageInfoUI.StartBombFlashEffect();
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

    public void Defeat()
    {
        Time.timeScale = 0f;
        defeatWindow.SetActive(true);
        defeatKillCount.text = stageInfoUI.killCountText.text;
        defeatTimeText.text = stageInfoUI.timeText.text;
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
    }

    public void StartBombFlash()
    {
        stageInfoUI.StartBombFlashEffect();
    }

    public void OpenStore()
    {
        Time.timeScale = 0f;
        storeWindow.SetActive(true);
    }

    public void CloseStore()
    {
        Time.timeScale = 1f;
        storeWindow.SetActive(false);
    }

    public void OpenPauseWindow()
    {
        Time.timeScale = 0f;
        pauseWindow.SetActive(true);
    }

    public void ClosePauseWindow()
    {
        Time.timeScale = 1f;
        pauseWindow.SetActive(false);
    }

    // 빌드 테스트 코드
    public void AddOneMinute()
    {
        gameTimer = Mathf.Clamp(gameTimer + 60f, 0, 600);
    }
    public void LevelUp()
    {
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
    //        
}
