using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIEventListener :
MonoBehaviour,
IPointerClickHandler,
IPointerEnterHandler,
IPointerExitHandler,
IPointerDownHandler,
IPointerUpHandler,
IDragHandler,
IDropHandler
{
    public UnityAction<GameObject> onClick;
    public UnityAction<GameObject, PointerEventData> onClick2;
    public UnityAction<GameObject, PointerEventData> onEnter;
    public UnityAction<GameObject, PointerEventData> onExit;
    public UnityAction<GameObject, PointerEventData> onDown;
    public UnityAction<GameObject, PointerEventData> onUp;
    public UnityAction<GameObject, PointerEventData> onDrag;
    public UnityAction<GameObject, PointerEventData> onDrop;

    public void OnPointerEnter(PointerEventData eventData) { if (onEnter != null) onEnter(gameObject, eventData); }
    public void OnPointerExit(PointerEventData eventData) { if (onExit != null) onExit(gameObject, eventData); }
    public void OnPointerDown(PointerEventData eventData) { if (onDown != null) onDown(gameObject, eventData); }
    public void OnPointerUp(PointerEventData eventData) { if (onUp != null) onUp(gameObject, eventData); }
    public void OnDrop(PointerEventData eventData) { if (onDrop != null) onDrop(gameObject, eventData); }

    public object parameter;

    int dragSafe = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.frameCount - dragSafe) < 10)
        {
            return;
        }
        if (onClick != null) onClick(gameObject);
        if (onClick2 != null) onClick2(gameObject, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {

        dragSafe = Time.frameCount;

        if (onDrag != null) onDrag(gameObject, eventData);
    }

    public static UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }
}
