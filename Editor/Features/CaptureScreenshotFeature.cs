using System;
using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public class CaptureScreenshotFeature
    {
        [MenuItem("Tools/VG/Capture Screenshot")]
        public static void CaptureScreenshot()
        {
            var defaultPath = EditorPrefs.GetString("VG_ScreenshotPath", Application.dataPath);
            var defaultFileName = $"{Application.productName} {DateTime.Now:yyyy-MM-dd HH-mm-ss}.png";

            var path = EditorUtility.SaveFilePanel("Save screenshot", defaultPath, defaultFileName, "png");
            if (string.IsNullOrEmpty(path))
                return;

            ScreenCapture.CaptureScreenshot(path);

            var pathWithoutFileName = path[..path.LastIndexOf('/')];
            if (pathWithoutFileName != defaultPath)
                EditorPrefs.SetString("VG_ScreenshotPath", pathWithoutFileName);
        }
    }
}