// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public class SnapSceneCameraToGameCameraFeature
    {
        [MenuItem("Tools/Editor/Snap Scene Camera to Game Camera")]
        public static void SnapSceneCamToGameCam()
        {
            var sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                var sceneCam = sceneView.camera;
                var gameCam = Camera.main;

                if (sceneCam != null && gameCam != null)
                {
                    sceneView.AlignViewToObject(gameCam.transform);
                    sceneView.orthographic = gameCam.orthographic;
                }
            }
        }
    }
}