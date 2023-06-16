using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomGradient))]
public class GradientDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CustomGradient gradient = (CustomGradient)
            fieldInfo.GetValue(property.serializedObject.targetObject);
        GUI.DrawTexture(position, gradient.GetTexture((int)position.width));
        GUI.Label(position, label.text);
    }
}
