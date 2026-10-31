using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiNavigationRedirectionChild : UiNavigationNode
{
    public int childIndex;

    public override void Selected()
    {
        if (transform.childCount > 0 && transform.GetChild(childIndex).TryGetComponent(out UiNavigationNode nav))
        {
            UiNavigationHandler.NavigateTo(nav);
        }
        else
        {
            UiNavigationHandler.NavigateUndo();
        }
    }
}
