using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject skillOne;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(skillOne, transform.position, Quaternion.identity);
        }
#endif
    }
}
