using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHomeManager : MonoBehaviour
{
    public int FinalStageNumber = 5;
    public Image currentImage;
    public TextMeshProUGUI currentStageName;

    public Sprite[] stageSprites;
    public string[] stageName;

    private int selectStageIndex = 1;

    private void Awake()
    {
        UpdateStageInfo();
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
}
