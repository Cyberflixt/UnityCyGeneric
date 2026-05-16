using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiNavigation : MonoBehaviour
{
    private UiSelector selectedElement;

    private void Start()
    {
        InputExt.actions["Move"].started += OnMoveStarted;
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}
