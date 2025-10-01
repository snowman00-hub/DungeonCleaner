using UnityEngine;

public class SkillBroomSlash : ActiveSkill
{
    public GameObject defaultBroom;
    public GameObject awakeningBroom;

    public Transform cube;
    public Transform awakeningCube;

    private float currentAngle;
    private BoxCollider boxCollider;
    private bool isSoundOn;

    protected override void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    protected override void OnEnable()
    {
        if(skillData.skillLevel == 6)
        {
            defaultBroom.SetActive(false);
            awakeningBroom.SetActive(true);
            cube = awakeningCube;
        }

        base.OnEnable();
        ScaleFromEdge(skillData.radius);
        SetStartAngle();
        isSoundOn = false;
    }

    private void Update()
    {
        if(!isSoundOn)
        {
            isSoundOn=true;
            AudioManager.Instance.BroomSlash();
        }

        transform.position = Player.Instance.transform.position;
        currentAngle += 120f / skillData.duration * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, currentAngle, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            var enemy = other.GetComponent<Enemy>();
            int finalDamage = Mathf.FloorToInt((skillData.damage + Player.Instance.data.atk) * Player.Instance.data.finalAttackMultiplier);
            enemy.OnDamage(finalDamage, enemy.transform.position, enemy.transform.forward);
            ActiveSkillManager.Instance.damageAmounts[skillName] += finalDamage;
            enemy.KnockBack(2.5f);
        }
    }

    private void SetStartAngle()
    {
        Vector3 forward = Player.Instance.transform.GetComponentInChildren<Animation>().transform.forward;
        Vector3 flatForward = new Vector3(forward.x, 0, forward.z).normalized;
        float angle = Mathf.Atan2(flatForward.x, flatForward.z) * Mathf.Rad2Deg;

        if (angle < 0) 
            angle += 360f;

        currentAngle = angle - 60f;
        transform.rotation = Quaternion.Euler(0, currentAngle, 0);
    }

    public void ScaleFromEdge(float scaleAmount)
    {
        Vector3 oldScale = cube.localScale;
        Vector3 newScale = oldScale;

        newScale.y = scaleAmount * 0.33f;
        float delta = newScale.y - oldScale.y;
        cube.position += (Vector3.forward * delta / 0.33f * 0.12f);
        cube.localScale = newScale;

        boxCollider.center = new Vector3 (0, 0.7f, scaleAmount / 2f);
        boxCollider.size = new Vector3(0.5f, 0.5f, scaleAmount);
    }
}
