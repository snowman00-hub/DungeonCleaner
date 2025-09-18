using System.Collections.Generic;
using UnityEngine;

public enum SkillName
{
    dustStorm,
    bubbleShield,
}

public class ActiveSkillManager : MonoBehaviour
{
    public static ActiveSkillManager Instance;
    public GameObject skillChest;
    public List<ActiveSkill> allSkillList;

    public ActiveSkill defaultSkill;
    [HideInInspector]
    public List<ActiveSkill> equippedSkills = new List<ActiveSkill>();

    private Dictionary<SkillName, Queue<GameObject>> skillPools = new Dictionary<SkillName, Queue<GameObject>>();
    private int poolSize = 10;

    private void Awake()
    {
        Instance = this;
        equippedSkills.Add(defaultSkill);
        skillChest.transform.position = Vector3.zero;

        foreach (var skill in allSkillList)
        {
            UpdateSkillData(skill, 1);

            if (skill.skillAttribute == SkillAttribute.Aura)
                continue;

            var queue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(skill.gameObject, skillChest.transform);
                obj.SetActive(false);
                queue.Enqueue(obj);

                var sk = obj.GetComponent<ActiveSkill>();
                sk.OnUsed += () =>
                {
                    queue.Enqueue(obj);
                    obj.SetActive(false);
                };
            }

            skillPools[skill.skillName] = queue;
        }
    }

    public void UpdateSkillData(ActiveSkill skill, int level)
    {
        var skillTable = DataTableManger.ActiveSkillTable;
        var levelData = skillTable.Get(skill.GetSkillLevelId(level));

        skill.skillData.skillLevel = levelData.CURRENT_LEVEL;
        skill.skillData.damage = levelData.DAMAGE;
        skill.skillData.radius = levelData.SKILL_RADIAL;
        skill.skillData.coolDown = levelData.SKILL_COOLTIME;
        skill.skillData.duration = levelData.SKILL_DURATION;
        skill.skillData.tickInterval = levelData.TICK_INTERVAL;
        skill.skillData.projectileCount = levelData.PROJECTILE_COUNT;
        skill.skillData.projectileSpeed = levelData.SKILL_SPEED;
    }

    public void EquipSkill(ActiveSkill skill,int level)
    {
        if(level == 1)
            equippedSkills.Add(skill);

        if (level == 1 && skill.skillAttribute == SkillAttribute.Aura)
            Instantiate(skill, Player.Instance.transform);

        UpdateSkillData(skill, level);
    }

    private void Update()
    {
        foreach (var skill in equippedSkills)
        {
            if (skill.skillAttribute == SkillAttribute.Aura)
                continue;

            if (skill.currentCoolDown > 0)
            {
                skill.currentCoolDown -= Time.deltaTime;
            }
            else
            {
                // 쿨타임이 다 됐으면 스킬 발동
                UseSkill(skill);
                skill.currentCoolDown = skill.skillData.coolDown;
            }
        }
    }

    private void UseSkill(ActiveSkill skill)
    {
        for (int projectileCount = 0; projectileCount < skill.skillData.projectileCount; projectileCount++)
        {
            if (skillPools.TryGetValue(skill.skillName, out var queue))
            {
                if (queue.Count == 0)
                {
                    for (int i = 0; i < poolSize; i++)
                    {
                        GameObject go = Instantiate(skill.gameObject, skillChest.transform);
                        go.SetActive(false);
                        queue.Enqueue(go);

                        var sk = go.GetComponent<ActiveSkill>();
                        sk.OnUsed += () =>
                        {
                            queue.Enqueue(go);
                            go.SetActive(false);
                        };
                    }
                }

                var temp = queue.Dequeue().gameObject;
                temp.transform.position = transform.position;
                temp.SetActive(true);
            }
        }
    }    
}
