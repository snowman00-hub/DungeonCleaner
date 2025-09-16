using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceSlot : MonoBehaviour
{
    public TextMeshProUGUI skillNameText;
    public Image skillImage;
    public TextMeshProUGUI skillDescText;
    public List<Image> starImages;

    private Button button;
    private ActiveSkill currentSkill;
    private int currentSkillLevel;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSlotChoice);
    }

    private void OnEnable()
    {
        int rand = Random.Range(0, ActiveSkillManager.Instance.allSkillList.Count);
        var skill = ActiveSkillManager.Instance.allSkillList[rand];

        currentSkill = skill;
        var skillTable = DataTableManger.ActiveSkillTable;
        ActiveSkillData levelData;
        if (ActiveSkillManager.Instance.equippedSkills.Contains(skill))
        {
            levelData = skillTable.Get(skill.GetSkillLevelId(skill.skillData.skillLevel + 1));
        }
        else
        {
            levelData = skillTable.Get(skill.GetSkillLevelId(1));
        }

        skillImage.sprite = skill.skillSprite;
        skillNameText.text = levelData.SKILL_NAME;
        skillDescText.text = levelData.DESCRIPTION;
        currentSkillLevel = levelData.CURRENT_LEVEL;
        for (int i = 0; i < currentSkillLevel; i++)
        {
            starImages[i].enabled = true;
        }
        StartCoroutine(FadeLoop());
    }

    private void OnSlotChoice()
    {
        StageInfoManager.Instance.CloseSkillChoice();
        ActiveSkillManager.Instance.ApplySkillLevel(currentSkill, currentSkill.skillData.skillLevel + 1);
    }

    private void OnDisable()
    {
        Color c = starImages[currentSkillLevel - 1].color;
        c.a = 1f;
        starImages[currentSkillLevel - 1].color = c;
    }

    private float elaspedTime = 0.4f;
    private float waitTime = 0.1f;

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(0f, 1f));
            yield return new WaitForSecondsRealtime(waitTime);

            yield return StartCoroutine(Fade(1f, 0f));
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < elaspedTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / elaspedTime;
            Color c = starImages[currentSkillLevel - 1].color;
            c.a = Mathf.Lerp(from, to, t);  
            starImages[currentSkillLevel - 1].color = c;      
            yield return null;
        }

        Color color = starImages[currentSkillLevel - 1].color;
        color.a = to;
        starImages[currentSkillLevel - 1].color = color;
    }
}
