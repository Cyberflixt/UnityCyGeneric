using System;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiNavigationHandler : MonoBehaviour
{
    private static Stack<UiNavigationNode> nodeHistory = new();
    private static UiNavigationNode currentNode = null;
    private static Dictionary<string, UiNavigationNode> checkpoints = new();

    public static void NavigateTo(UiNavigationNode node)
    {
        if (node)
        {
            if (currentNode != null)
                currentNode.Deselected();
            currentNode = node;
            nodeHistory.Push(node);
            node.Selected();
        }
        else
        {
            currentNode = null;
            nodeHistory.Clear();
        }
    }

    public static void NavigateUndo()
    {
        if (nodeHistory.Count == 0)
            return;

        nodeHistory.Pop();
        while (nodeHistory.Peek() is UiNavigationRedirectionChild or UiNavigationRedirectionTo)
            nodeHistory.Pop();

        if (nodeHistory.TryPop(out UiNavigationNode node))
        {
            NavigateTo(node);
        }
        else
        {
            currentNode = null;
        }
    }

    public static void NavigateUndoUntil(UiNavigationNode checkpoint)
    {
        if (nodeHistory.Count == 0)
            return;

        while (nodeHistory.Count > 0 && nodeHistory.Peek() != checkpoint)
            nodeHistory.Pop();

        if (nodeHistory.Count > 0)
        {
            nodeHistory.Pop();
            NavigateTo(checkpoint);
        }
        else
        {
            currentNode = null;
        }
    }

    public static void UndoUntilNotNull()
    {
        if (nodeHistory.Count > 0 && (!nodeHistory.Peek() || !nodeHistory.Peek().gameObject.activeInHierarchy))
        {
            while (nodeHistory.Count > 0 && (!nodeHistory.Peek() || !nodeHistory.Peek().gameObject.activeInHierarchy))
                nodeHistory.Pop();
            if (nodeHistory.Count > 0)
                NavigateTo(nodeHistory.Peek());
            else
                NavigateTo(null);
        }
    }

    public static void SaveCheckpoint(string key)
    {
        checkpoints[key] = currentNode;
    }

    public static void LoadCheckpoint(string key)
    {
        NavigateUndoUntil(checkpoints[key]);
    }

    private void Start()
    {
        InputExt.actions["Move"].performed += OnMoveChanged;
    }

    private void OnEnable()
    {
        InputExt.ReadyCallback(() =>
        {
            InputExt.actions["Interact"].started += OnInteractStarted;
        });
    }

    private void OnDisable()
    {
        InputExt.actions["Interact"].started -= OnInteractStarted;
    }

    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        if (currentNode != null)
        {
            if (currentNode.TryGetComponent(out UiNavigationInteractEvent interaction))
            {
                interaction.Interact();
            }
        }
    }

    private void OnMoveChanged(InputAction.CallbackContext context)
    {
        while (currentNode == null && nodeHistory.Count > 0)
        {
            NavigateUndo();
        }

        if (currentNode is UiNavigationNodeNeighbors nodeNeigh)
        {
            Vector2 move = InputExt.actions["Move"].ReadValue<Vector2>();
            if (move.x != 0 && move.y != 0)
                move = move.normalized;

            // Get current node world position
            RectTransform currentRect = currentNode.GetComponent<RectTransform>();
            Vector3 currentPos = currentRect.TransformPoint(currentRect.rect.center);

            UiNavigationNode bestNeighbor = null;
            float bestAlignment = 0;

            foreach (NavigationNodeNeighbor neighborKV in nodeNeigh.neighbors)
            {
                if (neighborKV.node)
                {
                    float alignment;
                    if (neighborKV.dir == Vector2.zero)
                    {
                        // Get neighbor world position
                        RectTransform neighborRect = neighborKV.node.GetComponent<RectTransform>();
                        Vector3 neighborPos = neighborRect.TransformPoint(neighborRect.rect.center);

                        // Get alignment
                        Vector2 dir = (neighborPos - currentPos).normalized;
                        float dist = (neighborPos - currentPos).sqrMagnitude;
                        alignment = Vector2.Dot(move, dir) / dist;
                    }
                    else
                    {
                        alignment = Vector2.Dot(move, neighborKV.dir);
                    }

                    if (alignment > bestAlignment)
                    {
                        bestAlignment = alignment;
                        bestNeighbor = neighborKV.node;
                    }
                }
            }

            if (bestAlignment > 0)
            {
                NavigateTo(bestNeighbor);
            }
        }
    }
}
