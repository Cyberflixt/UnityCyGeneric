using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UiNavigationOnEnable : UiNavigationNode
{
    public UiNavigationNode navigateTo;

    public override void Selected()
    {
        UiNavigationHandler.NavigateTo(navigateTo);
    }
    private void OnEnable()
    {
        Selected();
    }
}
