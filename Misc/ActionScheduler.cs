using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionScheduler : MonoBehaviour
{
    public static List<(float, Action)> actions = new();

    public static void Add(float delay, Action action)
    {
        actions.Add((Time.time + delay, action));
    }

    void Update()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (Time.time >= actions[i].Item1)
            {
                actions[i].Item2.Invoke();
                actions.RemoveAt(i);
                i--;
            }
        }
    }
}
