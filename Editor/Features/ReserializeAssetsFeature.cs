// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Linq;
using UnityEditor;

namespace VG.Editor.Features
{
    public class ReserializeAssetsFeature
    {
        [MenuItem("Assets/Reserialize All", false, 40)]
        private static void ReserializeAllAssets()
        {
            var pathes = AssetDatabase.GetAllAssetPaths().ToList();
            AssetDatabase.ForceReserializeAssets(pathes);
        }

        [MenuItem("Assets/Reserialize Selected", false, 40)]
        public static void ReserializeSelectedAssets()
        {
            var pathes = Selection.objects.Select(AssetDatabase.GetAssetPath).ToList();
            AssetDatabase.ForceReserializeAssets(pathes);
        }

        [MenuItem("Assets/Reserialize Selected", true)]
        private static bool ValidateReserializeSelectedAssets() =>
            EditorUtility.IsPersistent(Selection.activeGameObject);
    }
}