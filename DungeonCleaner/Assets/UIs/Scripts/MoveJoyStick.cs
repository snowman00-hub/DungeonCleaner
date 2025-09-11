using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MoveJoyStick : MonoBehaviour, IPointerDownHandler
{
    public RectTransform joystick;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("onpointerdown");
        var touchPosition = eventData.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystick.parent as RectTransform,
            touchPosition, eventData.enterEventCamera, out Vector2 position))
        {
            joystick.anchoredPosition = position;
        }
    }
}
