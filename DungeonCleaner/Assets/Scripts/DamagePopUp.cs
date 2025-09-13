using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Setup(int damage)
    {
        text.text = damage.ToString();
        gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }

        transform.position += Vector3.up * Time.deltaTime;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
