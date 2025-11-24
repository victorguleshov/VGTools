// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.IO;
using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public static class MarkAssetAsDirtyFeature
    {
        [MenuItem("Assets/Mark as Dirty", false, 39)]
        private static void MarkAsDirty()
        {
            foreach (var selected in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(selected);
                if (Directory.Exists(path))
                    MarkDirectoryAsDirty(path);
                else
                    EditorUtility.SetDirty(selected);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void MarkDirectoryAsDirty(string path)
        {
            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var asset = AssetDatabase.LoadAssetAtPath<Object>(file);
                if (asset != null) EditorUtility.SetDirty(asset);
            }
        }
    }
}