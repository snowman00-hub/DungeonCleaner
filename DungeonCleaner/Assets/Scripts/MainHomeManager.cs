using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHomeManager : MonoBehaviour
{
    public static MainHomeManager Instance { get; private set; }
    public static readonly string StageIndex = "StageIndex";

    public int FinalStageNumber = 5;
    public Image currentImage;
    public TextMeshProUGUI currentStageName;

    public Sprite[] stageSprites;
    public string[] stageName;

    private int selectStageIndex = 1;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI myMoneytext;
    public TextMeshProUGUI myJewelText;
    private int myMoney;
    private int myJewel;

    public GameObject stageRightButton;
    public GameObject stageLeftButton;

    public AudioSource sfxAudio;
    public AudioClip errorClip;    

    public int MyMoney
    {
        get { return myMoney; }
        set
        {
            myMoney = value;
            myMoneytext.text = myMoney.ToString();
            SaveLoadManager.Data.gold = myMoney;
        }
    }

    public int MyJewel
    {
        get { return myJewel; }
        set
        {
            myJewel = value;
            myJewelText.text = myJewel.ToString();
        }
    }

    private void Awake()
    {
        Instance = this;
        UpdateStageInfo();
#if UNITY_EDITOR
        Application.targetFrameRate = -1;
#else
        Application.targetFrameRate = 60;
        Debug.unityLogger.logEnabled = false;
#endif
    }

    private void Start()
    {
        if (!SaveLoadManager.Load())
        {
            SaveLoadManager.Save();
        }
        else
        {
            MyMoney = SaveLoadManager.Data.gold;
            MyJewel = SaveLoadManager.Data.jewel;
            playerNameText.text = SaveLoadManager.Data.PlayerName;
        }

        selectStageIndex = PlayerPrefs.GetInt(StageIndex, 1);
        UpdateStageInfo();
    }

    private void OnDisable()
    {
        SaveLoadManager.Data.gold = MyMoney;
        SaveLoadManager.Data.jewel = MyJewel;
        SaveLoadManager.Save();
    }

    public void UpdateStageInfo()
    {
        if (selectStageIndex == 1)
        {
            stageLeftButton.SetActive(false);
            stageRightButton.SetActive(true);
        }
        else if (selectStageIndex == FinalStageNumber)
        {
            stageLeftButton.SetActive(true);
            stageRightButton.SetActive(false);
        }
        else
        {
            stageLeftButton.SetActive(true);
            stageRightButton.SetActive(true);
        }

        currentImage.sprite = stageSprites[selectStageIndex - 1];
        currentStageName.text = stageName[selectStageIndex - 1];
    }

    public void PlusOneStageIndex()
    {
        if (selectStageIndex == FinalStageNumber)
            return;

        selectStageIndex++;
        PlayerPrefs.SetInt(StageIndex, selectStageIndex);
        UpdateStageInfo();
    }

    public void MinusOneStageIndex()
    {
        if (selectStageIndex == 1)
            return;

        selectStageIndex--;
        PlayerPrefs.SetInt(StageIndex, selectStageIndex);
        UpdateStageInfo();
    }

    public void StartStage()
    {
        SceneManager.LoadScene(selectStageIndex);
    }

    public void ResetSave()
    {
        SaveLoadManager.Data = new SaveDataV1();
        SaveLoadManager.Save();
        MyMoney = SaveLoadManager.Data.gold;
        MyJewel = SaveLoadManager.Data.jewel;
        playerNameText.text = SaveLoadManager.Data.PlayerName;
    }

    public void ErrorSound()
    {
        sfxAudio.PlayOneShot(errorClip, 3.0f);
    }

    public void ChangeName(TMP_InputField inputField)
    {
        playerNameText.text = inputField.text;
        SaveLoadManager.Data.PlayerName = inputField.text;
        SaveLoadManager.Save();
    }

    // 테스트 코드
    public void GetGold()
    {
        MyMoney += 50000;
    }
    public void GetJewel()
    {
        MyJewel += 300;
    }
    //
}