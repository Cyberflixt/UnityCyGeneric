
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Atttribute for show a field if other field is true or false.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
public class ShowIfIntAttribute : PropertyAttribute
{
    public string key;
    public int value;
    public bool useEqual;

    public ShowIfIntAttribute(string key, int value, bool useEqual = true)
    {
        this.key = key;
        this.value = value;
        this.useEqual = useEqual;
    }
}