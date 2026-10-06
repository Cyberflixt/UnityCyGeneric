using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Dim3))]
public class Dim3PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Support multi-object editing, undo/redo, and prefab overrides
        EditorGUI.BeginProperty(position, label, property);

        // Draw the main field label (e.g., "Size" or variable name) and retrieve remaining width
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Store original GUI settings
        int originalIndent = EditorGUI.indentLevel;
        float originalLabelWidth = EditorGUIUtility.labelWidth;

        // Reset indent for inline horizontal fields
        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 24f; // Compact width for "px", "x%", "y%" mini-labels

        // Get property handles
        SerializedProperty pxProp = property.FindPropertyRelative("px");
        SerializedProperty xPctProp = property.FindPropertyRelative("xPercentage");
        SerializedProperty yPctProp = property.FindPropertyRelative("yPercentage");

        // Calculate 3 equal column Rects with 4px spacing
        float spacing = 4f;
        float fieldWidth = (position.width - (spacing * 2f)) / 3f;

        Rect pxRect = new Rect(position.x, position.y, fieldWidth, position.height);
        Rect xPctRect = new Rect(position.x + fieldWidth + spacing, position.y, fieldWidth, position.height);
        Rect yPctRect = new Rect(position.x + (fieldWidth + spacing) * 2f, position.y, fieldWidth, position.height);

        // Render fields in a single row
        EditorGUI.PropertyField(pxRect, pxProp, new GUIContent("px", "Pixels"));
        EditorGUI.PropertyField(xPctRect, xPctProp, new GUIContent("x%", "Percentage of parent's width"));
        EditorGUI.PropertyField(yPctRect, yPctProp, new GUIContent("y%", "Percentage of parent's height"));

        // Restore original GUI settings
        EditorGUIUtility.labelWidth = originalLabelWidth;
        EditorGUI.indentLevel = originalIndent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
