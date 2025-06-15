using UnityEngine;

namespace VG.Extensions
{
    public static class LineRendererExtensions
    {
        public static void DrawTrajectory(this LineRenderer lineRenderer, Vector3 origin, Vector3 force, float drag = 0f)
        {
            Vector3[] segments = null;
            var numSegments = 0;
            var maxIterations = 10000;
            var maxSegmentCount = 10;
            var segmentStepModulo = 5f;

            var timestep = Time.fixedDeltaTime;

            var stepDrag = 1 - drag * timestep;
            var velocity = force * timestep;
            var gravity = Physics.gravity * (timestep * timestep);
            var position = origin;

            if (segments == null || segments.Length != maxSegmentCount)
            {
                segments = new Vector3[maxSegmentCount];
            }

            segments[0] = position;
            numSegments = 1;

            for (var i = 0; i < maxIterations && numSegments < maxSegmentCount; i++)
            {
                velocity += gravity;
                velocity *= stepDrag;

                position += velocity;

                if (i % segmentStepModulo == 0)
                {
                    segments[numSegments] = position;
                    numSegments++;
                }
            }

            lineRenderer.positionCount = numSegments;
            for (var i = 0; i < numSegments; i++)
            {
                lineRenderer.SetPosition(i, segments[i]);
            }
        }
    }
}