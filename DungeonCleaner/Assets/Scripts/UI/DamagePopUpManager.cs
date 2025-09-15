using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager Instance;
    public DamagePopup prefab;
    public Transform worldCanvas;

    private Queue<DamagePopup> pool = new Queue<DamagePopup>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < 50; i++)
        {
            var popup = Instantiate(prefab, worldCanvas);
            popup.gameObject.SetActive(false);
            pool.Enqueue(popup);
        }
    }

    public void ShowDamage(Vector3 position, int damage)
    {
        DamagePopup popup;
        if (pool.Count > 0)
        {
            popup = pool.Dequeue();
        }
        else
        {
            popup = Instantiate(prefab, worldCanvas);
        }

        popup.Setup(damage);
        popup.transform.position = position + Vector3.up * 2f;
        StartCoroutine(HideAfterTime(popup, 0.5f));
    }

    private IEnumerator HideAfterTime(DamagePopup popup, float time)
    {
        yield return new WaitForSeconds(time);
        popup.Hide();
        pool.Enqueue(popup);
    }
}
