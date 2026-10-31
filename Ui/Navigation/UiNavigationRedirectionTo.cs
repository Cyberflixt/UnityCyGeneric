using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiNavigationRedirectionTo : UiNavigationNode
{
    public UiNavigationNode navigateTo;

    public override void Selected()
    {
        UiNavigationHandler.NavigateTo(navigateTo);
    }
}
