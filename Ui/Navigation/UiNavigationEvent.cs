using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// UiNavigationNodeNeighbors with events when selected or deselected
/// </summary>
public class UiNavigationEvent : UiNavigationNodeNeighbors
{
    public UnityEvent onSelected;
    public UnityEvent onDeselected;

    public override void Selected()
    {
        base.Selected();
        onSelected?.Invoke();
    }
    public override void Deselected()
    {
        base.Deselected();
        onDeselected?.Invoke();
    }
}
