using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> ClickAction;
    public Action<PointerEventData> DragAction;

    public void OnDrag(PointerEventData eventData)
    {
        if (DragAction != null)
        {
            DragAction.Invoke(eventData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ClickAction != null)
        {
            ClickAction.Invoke(eventData);
        }
    }
}
