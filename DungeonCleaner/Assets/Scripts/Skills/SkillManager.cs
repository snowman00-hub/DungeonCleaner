using System.Collections.Generic;
using UnityEngine;

public enum SkillName
{
    dustStorm,
}

public class SkillManager : MonoBehaviour
{
    public GameObject skillChest;
    public List<Skill> skills;

    private Dictionary<SkillName, Queue<GameObject>> skillPools = new Dictionary<SkillName, Queue<GameObject>>();
    private int poolSize = 10;

    private void Awake()
    {
        skillChest.transform.position = Vector3.zero;

        foreach (var skill in skills)
        {
            var queue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(skill.gameObject, skillChest.transform);
                obj.SetActive(false);
                queue.Enqueue(obj);

                var sk = obj.GetComponent<Skill>();
                sk.OnUsed += () =>
                {
                    queue.Enqueue(obj);
                    obj.SetActive(false);
                };
            }

            skillPools[skill.skillName] = queue;
        }
    }

    private void Update()
    {
        foreach (var skill in skills)
        {
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

    private void UseSkill(Skill skill)
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

                        var sk = go.GetComponent<Skill>();
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
