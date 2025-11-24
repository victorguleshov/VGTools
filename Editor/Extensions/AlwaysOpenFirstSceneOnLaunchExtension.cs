// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VG.Editor.Extensions
{
    [InitializeOnLoad]
    public class AlwaysOpenFirstSceneOnLaunchExtension
    {
        private const string AlwaysOpenPrefKey = "AlwaysOpenFirstSceneOnLaunch";
        private const string MenuPath = "Tools/Editor/Always Open First Scene on Editor Launch";
        private const string FirstSessionFlagKey = "AlwaysOpenFirstSceneOnLaunch_AlreadyRun";

        static AlwaysOpenFirstSceneOnLaunchExtension()
        {
            if (IsFirstSession)
                return;

            IsFirstSession = true;
            EditorApplication.delayCall += OpenFirstSceneIfNeeded;
        }

        private static bool IsFirstSession { get => SessionState.GetBool(FirstSessionFlagKey, false); set => SessionState.SetBool(FirstSessionFlagKey, value); }

        private static bool AlwaysOpenFirstSceneOnLaunch { get => EditorPrefs.GetBool(AlwaysOpenPrefKey, false); set => EditorPrefs.SetBool(AlwaysOpenPrefKey, value); }

        [MenuItem(MenuPath, priority = 100)]
        private static void ToggleAlwaysOpenFirstSceneOnLaunch()
        {
            AlwaysOpenFirstSceneOnLaunch = !AlwaysOpenFirstSceneOnLaunch;
            Debug.Log($"'Always Open First Scene' is now {(AlwaysOpenFirstSceneOnLaunch ? "enabled" : "disabled")}.");
        }

        [MenuItem(MenuPath, true)]
        private static bool ToggleAlwaysOpenFirstSceneOnLaunchValidate()
        {
            Menu.SetChecked(MenuPath, AlwaysOpenFirstSceneOnLaunch);
            return true;
        }

        private static void OpenFirstSceneIfNeeded()
        {
            var shouldOpen = AlwaysOpenFirstSceneOnLaunch ||
                             (SceneManager.sceneCount == 1 && string.IsNullOrEmpty(SceneManager.GetActiveScene().path));

            if (!shouldOpen)
                return;

            var scenes = EditorBuildSettings.scenes;
            if (scenes.Length == 0)
            {
                Debug.LogWarning("No scenes found in Build Settings. Cannot auto-open first scene.");
                return;
            }

            var firstEnabledScene = scenes.FirstOrDefault(scene => scene.enabled);
            if (firstEnabledScene == null || string.IsNullOrEmpty(firstEnabledScene.path))
            {
                Debug.LogWarning("No enabled scenes found in Build Settings or the first enabled scene has an invalid path. Cannot auto-open.");
                return;
            }

            if (SceneManager.GetActiveScene().path == firstEnabledScene.path)
            {
                Debug.Log($"First enabled scene '{firstEnabledScene.path}' is already open.");
                return;
            }

            try
            {
                Debug.Log($"Opening first enabled scene from Build Settings: {firstEnabledScene.path}");
                EditorSceneManager.OpenScene(firstEnabledScene.path, OpenSceneMode.Single);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to open scene '{firstEnabledScene.path}'. Error: {ex.Message}");
            }
        }
    }
}