using UnityEngine;

public class SafeAreaPositioner : MonoBehaviour
{
    public RectTransform target;

    [Range(0f, 1f)]
    public float Xfactor; 
    [Range(0f, 1f)]
    public float Yfactor;

    public void ApplySafeAreaPosition()
    {
        Rect safe = Screen.safeArea;

        float posX = safe.x + safe.width * Xfactor;
        float posY = safe.y + safe.height * Yfactor;

        // Canvas 기준 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            target.parent as RectTransform,
            new Vector2(posX, posY),
            null,
            out localPoint
        );

        target.anchoredPosition = localPoint;
    }
}
