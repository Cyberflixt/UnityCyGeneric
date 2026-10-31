using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UiButtonsHoldAxis : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Vector2 axis = Vector2.right;

    [NonSerialized] public bool down = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        down = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        down = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        down = false;
    }
}
