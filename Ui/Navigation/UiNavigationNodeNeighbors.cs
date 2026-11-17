using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NavigationNodeNeighbor
{
    [Header("Set dir to (0,0) for automatic direction")]
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
    public List<NavigationNodeNeighbor> neighbors;
}
