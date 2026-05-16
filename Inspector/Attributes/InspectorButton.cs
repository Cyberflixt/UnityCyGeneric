using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

/// <summary>
/// Adds a button to a method in the Unity Inspector.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ButtonAttribute : Attribute
{
    public string Name { get; }
    
    public ButtonAttribute(string name = null)
    {
        Name = name;
    }
}
