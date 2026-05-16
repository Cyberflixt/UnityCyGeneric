using System.Collections.Generic;
using UnityEngine;

public class UiNavigationLink
{
    public Vector2 direction = Vector2.zero;
    public UiSelector selector;
}

public class UiSelector : MonoBehaviour
{
    [SerializeField] private List<UiNavigationLink> navigationLinks;

    
}
