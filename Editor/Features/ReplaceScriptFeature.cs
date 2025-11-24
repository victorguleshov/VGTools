// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public class ReplaceScriptFeature : EditorWindow
    {
        private static EditorApplication.CallbackFunction _updateCallback;
        private static int _pickerControlID;
        private static MonoScript _newScript;

        [MenuItem("CONTEXT/Component/Replace Script", validate = true)]
        private static bool ValidateReplaceScript(MenuCommand command)
        {
            if (command?.context is not Component component)
                return false;

            var isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(component);
            if (isPrefabInstance)
            {
                // Check if this specific component is from the prefab
                var prefabComponent = PrefabUtility.GetCorrespondingObjectFromSource(component);
                if (prefabComponent != null)
                    return false;
            }

            return true;
        }

        [MenuItem("CONTEXT/Component/Replace Script")]
        private static void ReplaceScript(MenuCommand command)
        {
            if (command?.context is not Component component)
            {
                Debug.LogError("No component selected.");
                return;
            }

            CleanupCallback(); // Ensure previous callback is cleaned up

            _pickerControlID = GUIUtility.GetControlID(FocusType.Passive);
            _newScript = null;
            _updateCallback = () => OnObjectPickerUpdated(component);
            EditorGUIUtility.ShowObjectPicker<MonoScript>(null, false, "", _pickerControlID);
            EditorApplication.update += _updateCallback;
        }

        private static void OnObjectPickerUpdated(Component component)
        {
            if (component == null) // Additional safety check
            {
                CleanupCallback();
                return;
            }

            if (EditorGUIUtility.GetObjectPickerControlID() == _pickerControlID)
            {
                _newScript = EditorGUIUtility.GetObjectPickerObject() as MonoScript;
                return;
            }

            if (EditorGUIUtility.GetObjectPickerControlID() != 0)
                return;

            CleanupCallback();

            if (_newScript == null)
            {
                Debug.LogError("No script selected.");
                return;
            }

            try
            {
                var serializedObject = new SerializedObject(component);
                var scriptProperty = serializedObject.FindProperty("m_Script");
                scriptProperty.objectReferenceValue = _newScript;
                serializedObject.ApplyModifiedProperties();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to replace script: {e.Message}");
            }
        }

        private static void CleanupCallback()
        {
            if (_updateCallback != null)
            {
                EditorApplication.update -= _updateCallback;
                _updateCallback = null;
            }
        }
    }
}