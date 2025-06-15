using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public static class OpenPersistentDataPathFeature
    {
        [MenuItem("Tools/VG/Open Persistent Data Path")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}