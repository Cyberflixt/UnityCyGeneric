
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ShowIfIntAttribute))]
public class ShowIfIntDrawer : PropertyDrawer
{
#if UNITY_EDITOR
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfIntAttribute condHAtt = (ShowIfIntAttribute)attribute;
        bool enabled = GetConditionalSourceValue(property, condHAtt);
        //GUI.enabled = enabled;

        if (enabled)
        {
            // Enabled, normal
            EditorGUI.PropertyField(position, property, label, true);
        }
        // else, hidden
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfIntAttribute target = (ShowIfIntAttribute)attribute;
        bool enabled = GetConditionalSourceValue(property, target);

        // if is enable draw the label
        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        else
        {
            // Hide
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    /// <summary>
    /// Get if the conditional what expected is true.
    /// </summary>
    /// <param name="property"> is used for get the value of the property and check if return enable true or false </param>
    /// <param name="target"> is the attribute what contains the values what we need </param>
    /// <returns> only if the field y is same to the value expected return true</returns>
    private bool GetConditionalSourceValue(SerializedProperty property, ShowIfIntAttribute target)
    {
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, target.key);
        SerializedProperty propertyValue = property.serializedObject.FindProperty(conditionPath);

        // Found serialized field?
        if (propertyValue != null)
        {
            if (target.useEqual)
                return propertyValue.intValue == target.value;
            return propertyValue.intValue != target.value;
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        // Find property using reflection
        UnityEngine.Object targetObject = property.serializedObject.targetObject;
        Type targetType = targetObject.GetType();
        PropertyInfo propertyInfo = targetType.GetProperty(target.key, flags);

        // Found bool property?
        if (propertyInfo != null && propertyInfo.PropertyType == typeof(int))
        {
            if (target.useEqual)
                return (int)propertyInfo.GetValue(targetObject) == target.value;
            return (int)propertyInfo.GetValue(targetObject) != target.value;
        }

        // Neither field nor property found
        Debug.LogWarning($"ShowIf: Property or field '{target.key}' not found on {targetType}");
        return false;
    }

#endif
}