﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Sounds.Editor
{
     [CustomPropertyDrawer(typeof(ClipNameDropdownAttribute))]
    public class ClipNameDropdownDrawer : PropertyDrawer
    {
        private bool m_IsCustom;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ClipNameDropdownAttribute dropdownAttribute = attribute as ClipNameDropdownAttribute;

            if (dropdownAttribute == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(position, "StringDropdownAttribute can only be used with string fields", MessageType.Error);
                return;
            }

            List<string> dropdownOptions = new();

            foreach (AudioClipGroup clipGroup in ((SoundSystemAsset) property.serializedObject.targetObject).ClipGroups)
            {
                dropdownOptions.AddRange(clipGroup.Clips.Select(clip => clip != null ? clip.name : null).Where(val => val != null));
            }

            int currentIndex = dropdownOptions.FindIndex(d => d == property.stringValue);

            Rect dropdownPosition = position;
            float checkBoxWidth = 60f;
            dropdownPosition.width -= checkBoxWidth;
            Rect checkboxPosition = new(position.x + dropdownPosition.width + 5f, position.y, checkBoxWidth, position.height);
            m_IsCustom = GUI.Toggle(checkboxPosition, m_IsCustom || dropdownOptions.Count == 0, "Custom");

            if (m_IsCustom)
            {
                property.stringValue = EditorGUI.TextField(dropdownPosition, label, property.stringValue);
            }
            else
            {
                if (dropdownOptions.Count == 0)
                {
                    EditorGUI.HelpBox(dropdownPosition, "Add AudioFiles first", MessageType.Info);
                    return;
                }

                EditorGUI.BeginChangeCheck();
                int newIndex = EditorGUI.Popup(dropdownPosition, label.text, currentIndex, dropdownOptions.ToArray());

                if (newIndex == -1 && !string.IsNullOrEmpty(property.stringValue))
                    m_IsCustom = true;

                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = dropdownOptions[newIndex];
                }
            }
        }
    }
}