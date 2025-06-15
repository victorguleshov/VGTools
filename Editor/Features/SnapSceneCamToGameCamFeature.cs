using UnityEditor;
using UnityEngine;

namespace PlatformerDemo.Editor
{
    public class SnapSceneCamToGameCamFeature
    {
        [MenuItem("Tools/Snap SceneCam To GameCam")]
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