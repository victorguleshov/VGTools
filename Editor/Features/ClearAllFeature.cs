// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.IO;
using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public static class ClearAllFeature
    {
        [MenuItem("Tools/Editor/Clear All")]
        public static void ClearAll()
        {
            ClearPlayerPrefs();
            ClearPersistentData();
        }

        [MenuItem("Tools/Editor/Clear Player Prefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Tools/Editor/Clear Persistent Data")]
        public static void ClearPersistentData()
        {
            foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
            {
                var dataDir = new DirectoryInfo(directory);
                dataDir.Delete(true);
            }

            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
            {
                var fileInfo = new FileInfo(file);
                fileInfo.Delete();
            }
        }
    }
}