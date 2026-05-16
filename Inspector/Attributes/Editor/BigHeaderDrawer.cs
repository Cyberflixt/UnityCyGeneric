using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BigHeaderAttribute))]
public class BigHeaderDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2.2f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (BigHeaderAttribute)attribute;
        
        // Text field style
        GUIStyle style = new GUIStyle(EditorStyles.textField)
        {
            fontSize = attr.fontSize,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        EditorGUI.BeginProperty(position, label, property);
        property.stringValue = EditorGUI.TextField(position, property.stringValue, style);
        EditorGUI.EndProperty();
    }
}
