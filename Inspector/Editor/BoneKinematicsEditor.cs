using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Allow multi-objects editing
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(BoneJiggleRod))]
public class MyComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
