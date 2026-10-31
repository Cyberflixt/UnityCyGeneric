using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UiNavigationEvent : UiNavigationNodeNeighbors
{
    public UnityEvent onSelected;
    public UnityEvent onDeselected;

    public override void Selected()
    {
        onSelected?.Invoke();
    }
    public override void Deselected()
    {
        onDeselected?.Invoke();
    }
}
