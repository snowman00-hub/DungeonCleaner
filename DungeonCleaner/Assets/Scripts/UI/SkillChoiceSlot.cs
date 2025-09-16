using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceSlot : MonoBehaviour
{
    public TextMeshProUGUI skillNameText;
    public Image skillImage;
    public TextMeshProUGUI skillDescText;
    public List<Image> starImages;

    public List<Skill> skillList;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => StageInfoManager.Instance.CloseSkillChoice());
    }

    private void OnEnable()
    {
        int rand = Random.Range(0,skillList.Count);
        var skill = skillList[rand];
        var skillTable = DataTableManger.ActiveSkillTable;
        var levelData = skillTable.Get(skill.GetSkillLevelId(skill.skillData.skillLevel));

        skillImage.sprite = skill.skillSprite;
        skillNameText.text = levelData.SKILL_NAME;
        skillDescText.text = levelData.DESCRIPTION;
        for(int i= 0; i < levelData.CURRENT_LEVEL; i++) 
        {
            starImages[i].enabled = true;
        }
    }
}
