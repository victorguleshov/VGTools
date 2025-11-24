// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public class CaptureScreenshotFeature
    {
        private const string ScreenshotPathPrefKey = "ScreenshotPath";
        private const string MenuPath = "Tools/Editor/Capture Screenshot";

        // --- Settings ---
        private static string ScreenshotPath
        {
            get => EditorPrefs.GetString(ScreenshotPathPrefKey, Application.dataPath); // Default to false
            set => EditorPrefs.SetString(ScreenshotPathPrefKey, value);
        }

        [MenuItem(MenuPath)]
        public static void CaptureScreenshot()
        {
            var defaultPath = ScreenshotPath;
            var defaultFileName = $"{Application.productName} {DateTime.Now:yyyy-MM-dd HH-mm-ss}.png";

            var path = EditorUtility.SaveFilePanel("Save screenshot", ScreenshotPath, defaultFileName, "png");
            if (string.IsNullOrEmpty(path))
                return;

            ScreenCapture.CaptureScreenshot(path);

            var pathWithoutFileName = path[..path.LastIndexOf('/')];
            if (pathWithoutFileName != defaultPath)
                ScreenshotPath = pathWithoutFileName;
        }
    }
}