using UnityEngine;

public class RelativeWidth : MonoBehaviour
{
    [Range(0f, 1f)]
    public float widthRatio = 0.5f; // 부모 Width의 몇 %?

    private RectTransform rect;
    private RectTransform parent;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        parent = rect.parent as RectTransform;
    }

    private void Update()
    {
        if (parent != null)
        {
            float newWidth = parent.rect.width * widthRatio;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        }
    }
}
