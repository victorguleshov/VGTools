// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public static class OpenPersistentDataPathFeature
    {
        [MenuItem("Tools/Editor/Open Persistent Data Path")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}