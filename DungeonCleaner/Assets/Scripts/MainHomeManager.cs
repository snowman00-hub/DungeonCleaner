using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHomeManager : MonoBehaviour
{
    public static MainHomeManager Instance { get; private set; }

    public int FinalStageNumber = 5;
    public Image currentImage;
    public TextMeshProUGUI currentStageName;

    public Sprite[] stageSprites;
    public string[] stageName;

    private int selectStageIndex = 1;

    public TextMeshProUGUI myMoneytext;
    public TextMeshProUGUI myJewelText;
    private int myMoney;
    private int myJewel;

    public int MyMoney
    {
        get { return  myMoney; }
        set
        {
            myMoney = value;
            myMoneytext.text = myMoney.ToString();
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
        }
    }

    private void OnDisable()
    {
        SaveLoadManager.Data.gold = MyMoney;
        SaveLoadManager.Data.jewel = MyJewel;
        SaveLoadManager.Save();
    }

    public void UpdateStageInfo()
    {
        currentImage.sprite = stageSprites[selectStageIndex -1];
        currentStageName.text = stageName[selectStageIndex - 1];
    }

    public void PlusOneStageIndex()
    {
        if (selectStageIndex == FinalStageNumber)
            return;

        selectStageIndex++;
        UpdateStageInfo();
    }

    public void MinusOneStageIndex()
    {
        if (selectStageIndex == 1)
            return;

        selectStageIndex--;
        UpdateStageInfo();
    }

    public void StartStage()
    {
        SceneManager.LoadScene(selectStageIndex);
    }

    // 테스트 코드
    public void ResetSave()
    {
        SaveLoadManager.Data = new SaveDataV1();
        SaveLoadManager.Save();
        MyMoney = SaveLoadManager.Data.gold;
        MyJewel = SaveLoadManager.Data.jewel;
    }
    public void GetGold()
    {
        MyMoney += 5000;
    }
    public void GetJewel()
    {
        MyJewel += 300;
    }
    //
}