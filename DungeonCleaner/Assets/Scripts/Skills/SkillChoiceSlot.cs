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
    private ActiveSkill activeSkill;
    private PassiveSkill passiveSkill;
    private int currentSkillLevel;

    public bool IsActive;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSlotChoice);
    }

    private void StartEffect()
    {
        for (int i = 0; i < currentSkillLevel; i++)
        {
            starImages[i].enabled = true;
        }
        StartCoroutine(FadeLoop());
    }

    public void ShowActiveSkill(ActiveSkill skill)
    {
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

        activeSkill = skill;
        skillImage.sprite = skill.skillSprite;
        skillNameText.text = levelData.SKILL_NAME;
        skillDescText.text = levelData.DESCRIPTION;
        currentSkillLevel = levelData.CURRENT_LEVEL;
        StartEffect();
    }

    public void ShowPassiveSkill(PassiveSkill skill)
    {
        PassiveSkillData showSkillData;
        if(PassiveSkillManager.Instance.equippedSkills.Contains(skill))
        {
            showSkillData = DataTableManger.PassiveSkillTable.Get(skill.GetSkillLevelId(skill.data.SKILL_LEVEL + 1));
        }
        else
        {
            showSkillData = skill.data;
        }

        passiveSkill = skill;
        skillImage.sprite = skill.sprite;
        skillNameText.text = showSkillData.SKILL_NAME;
        skillDescText.text = showSkillData.DESCRIPTION;
        currentSkillLevel = showSkillData.SKILL_LEVEL;
        StartEffect();
    }

    private void OnSlotChoice()
    {
        StageInfoManager.Instance.CloseSkillChoice();
        if (IsActive)
        {
            ActiveSkillManager.Instance.EquipSkill(activeSkill, currentSkillLevel);
        }
        else
        {
            if (PassiveSkillManager.Instance.equippedSkills.Contains(passiveSkill))
            {
                PassiveSkillManager.Instance.EquipSkill(passiveSkill, passiveSkill.data.SKILL_LEVEL + 1);
            }
            else
            {
                PassiveSkillManager.Instance.EquipSkill(passiveSkill, 1);
            }
        }

        AudioManager.Instance.Click();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
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
