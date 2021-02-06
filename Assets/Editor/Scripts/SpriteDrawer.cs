#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Sprite))]
public class SpriteDrawer : PropertyDrawer
{

    public const float size = 100;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var previousIndentLevel = EditorGUI.indentLevel;

        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.indentLevel = 0;

        var spriteRect = new Rect(position.x, position.y, size, size);

        property.objectReferenceValue =
            EditorGUI.ObjectField(spriteRect, property.objectReferenceValue, typeof(Sprite), false);

        EditorGUI.EndProperty();

        EditorGUI.indentLevel = previousIndentLevel;

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return size;
    }

}
#endif
