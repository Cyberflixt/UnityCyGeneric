using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct NavigationNodeNeighbor
{
    public Vector2 dir;
    public UiNavigationNode node;

    public NavigationNodeNeighbor(Vector2 dir, UiNavigationNode node)
    {
        this.dir = dir;
        this.node = node;
    }
}

public class UiNavigationNodeNeighbors : UiNavigationNode
{
    public NavigationNodeNeighbor[] neighbors;
}
