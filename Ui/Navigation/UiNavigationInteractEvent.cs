using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UiNavigationInteractEvent : MonoBehaviour
{
    public UnityEvent onInteract;

    public void Interact()
    {
        onInteract?.Invoke();
    }
}
