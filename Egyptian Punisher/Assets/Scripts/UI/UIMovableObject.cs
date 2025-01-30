using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UIMovableObject : MonoBehaviour, IScreenListener, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform originParent;
    [SerializeField] private Transform dragParent;

    [Space]
    [Range(0, 1), SerializeField] private float dragAlpha = 0.6f;

    private CanvasGroup group;

    public void OnScreenInit()
    {
        group = GetComponent<CanvasGroup>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(dragParent);
        group.alpha = dragAlpha;
        OnBegin();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        OnDrag();
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originParent);
        group.alpha = 1;
        OnEnd();
    }

    protected virtual void OnBegin() { }
    protected virtual void OnDrag() { }
    protected virtual void OnEnd() { }
}
