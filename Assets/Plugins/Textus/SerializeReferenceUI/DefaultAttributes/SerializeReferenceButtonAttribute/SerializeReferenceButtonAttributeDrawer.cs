#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

[CustomPropertyDrawer(typeof(SerializeReferenceButtonAttribute))]
public class SerializeReferenceButtonAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float spacing = 2;
        float propHeight = EditorGUI.GetPropertyHeight(property, true);

        return line + spacing + propHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float spacing = 2;

        EditorGUI.BeginProperty(position, label, property);

        // --- ЛИНИЯ С КНОПКАМИ ---
        Rect row = new Rect(position.x, position.y, position.width, line);

        Rect findBtn = new Rect(row.x, row.y, 36, line);
        Rect openBtn = new Rect(row.x + 36, row.y, 36, line);
        Rect changeBtn = new Rect(row.x + 72, row.y, row.width - 72, line);

        if (GUI.Button(findBtn, "Find", EditorStyles.miniButtonLeft))
        {
            PingScript(property);
        }

        if (GUI.Button(openBtn, "Open", EditorStyles.miniButtonMid))
        {
            OpenScript(property);
        }

        var typeRestrictions =
            SerializedReferenceUIDefaultTypeRestrictions.GetAllBuiltInTypeRestrictions(fieldInfo);
        property.DrawSelectionButtonForManagedReference(changeBtn, typeRestrictions);

        // --- ПОЛЕ СВОЙСТВА ---
        Rect propRect = new Rect(
            position.x,
            position.y + line + spacing,
            position.width,
            position.height - line - spacing
        );

        EditorGUI.PropertyField(propRect, property, true);

        EditorGUI.EndProperty();
    }

    // ------------------------

    private void PingScript(SerializedProperty property)
    {
        MonoScript script = FindMonoScript(property);
        if (script == null) return;

        Selection.activeObject = script;
        EditorGUIUtility.PingObject(script);
    }

    private void OpenScript(SerializedProperty property)
    {
        MonoScript script = FindMonoScript(property);
        if (script == null) return;

        AssetDatabase.OpenAsset(script);
    }

    private MonoScript FindMonoScript(SerializedProperty property)
    {
        if (property.managedReferenceValue == null)
        {
            Debug.LogWarning("Сначала назначьте класс в SerializeReference.");
            return null;
        }

        Type type = property.managedReferenceValue.GetType();
        string[] guids = AssetDatabase.FindAssets($"{type.Name} t:MonoScript");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

            if (script != null && script.GetClass() == type)
                return script;
        }

        Debug.LogWarning("Скрипт не найден: " + type.FullName);
        return null;
    }
}
#endif
