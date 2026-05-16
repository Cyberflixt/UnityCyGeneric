using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WarningBoxAttribute))]
public class WarningBoxDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        WarningBoxAttribute warning = (WarningBoxAttribute)attribute;

        // Calculate height for help box
        Rect helpBoxRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight * 2f);
        EditorGUI.HelpBox(helpBoxRect, warning.Message, MessageType.Warning);

        // Adjust position for property field below the help box
        Rect propertyRect = new Rect(position.x, position.y + helpBoxRect.height + 2, position.width, EditorGUI.GetPropertyHeight(property, label, true));
        EditorGUI.PropertyField(propertyRect, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2f + EditorGUI.GetPropertyHeight(property, label, true) + 2f;
    }
}
