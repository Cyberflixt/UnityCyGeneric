using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

#if UNITY_EDITOR
[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonAttributeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MonoBehaviour monoBehaviour = (MonoBehaviour)target;
        Type type = monoBehaviour.GetType();

        foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static))
        {
            var attribute = method.GetCustomAttribute<ButtonAttribute>();
            if (attribute != null)
            {
                string buttonName = string.IsNullOrEmpty(attribute.Name) ? method.Name : attribute.Name;
                if (GUILayout.Button(buttonName))
                {
                    method.Invoke(monoBehaviour, null);
                }
            }
        }
    }
}
#endif
