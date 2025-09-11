using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;

    private float radius;
    private SafeAreaPositioner positioner;

    public Vector2 Input { get; private set; }

    private void Start()
    {
        radius = background.rect.width * 0.5f;
        positioner = GetComponent<SafeAreaPositioner>();
        positioner.ApplySafeAreaPosition();
    }

    private void Update()
    {
#if UNITY_EDITOR
        //Debug.Log(Input);
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var touchPosition = eventData.position;

        if (RectTransformUtility.RectangleContainsScreenPoint(
            handle, eventData.position, eventData.enterEventCamera))
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background.parent as RectTransform,
            touchPosition, eventData.enterEventCamera, out Vector2 position))
        {
            background.anchoredPosition = position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        var touchPosition = eventData.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, touchPosition, eventData.enterEventCamera, out Vector2 position))
        {
            var delta = position;
            delta = Vector2.ClampMagnitude(delta, radius);
            handle.anchoredPosition = delta;
            Input = delta / radius;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        positioner.ApplySafeAreaPosition();
    }
}
