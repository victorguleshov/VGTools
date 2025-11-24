// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    [ExecuteAlways]
    [AddComponentMenu("Layout/Canvas Scaler Expand Clamped", 101)]
    [DisallowMultipleComponent]
    public sealed class CanvasScalerExpandClamped : UIBehaviour
    {
        [SerializeField] private Vector2 m_ReferenceResolution = new(1080, 1920);
        [SerializeField] private float m_ReferencePixelsPerUnit = 100;

        private Canvas mCanvas;
        [NonSerialized] private float mPrevReferencePixelsPerUnit = 100;
        [NonSerialized] private float mPrevScaleFactor = 1;

        public float ReferencePixelsPerUnit
        {
            get => m_ReferencePixelsPerUnit;
            set => m_ReferencePixelsPerUnit = value;
        }

        public Vector2 ReferenceResolution
        {
            get => m_ReferenceResolution;
            set
            {
                m_ReferenceResolution = value;

                const float kMinimumResolution = 0.00001f;

                if (m_ReferenceResolution.x is > -kMinimumResolution and < kMinimumResolution)
                    m_ReferenceResolution.x = kMinimumResolution * Mathf.Sign(m_ReferenceResolution.x);
                if (m_ReferenceResolution.y is > -kMinimumResolution and < kMinimumResolution)
                    m_ReferenceResolution.y = kMinimumResolution * Mathf.Sign(m_ReferenceResolution.y);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            mCanvas = GetComponent<Canvas>();
            Handle();
            Canvas.preWillRenderCanvases += Canvas_preWillRenderCanvases;
        }

        protected override void OnDisable()
        {
            SetScaleFactor(1);
            SetReferencePixelsPerUnit(100);
            Canvas.preWillRenderCanvases -= Canvas_preWillRenderCanvases;
            base.OnDisable();
        }

        private void Canvas_preWillRenderCanvases()
        {
            Handle();
        }

        private void Handle()
        {
            if (mCanvas == null || !mCanvas.isRootCanvas)
                return;

            if (mCanvas.renderMode == RenderMode.WorldSpace) return;

            HandleScaleWithScreenSize();
        }

        private void HandleScaleWithScreenSize()
        {
            var screenSize = mCanvas.renderingDisplaySize;

            // Multiple display support only when not the main display. For display 0 the reported
            // resolution is always the desktops resolution since its part of the display API,
            // so we use the standard none multiple display method. (case 741751)
            var displayIndex = mCanvas.targetDisplay;
            if (displayIndex > 0 && displayIndex < Display.displays.Length)
            {
                var display = Display.displays[displayIndex];
                screenSize = new Vector2(display.renderingWidth, display.renderingHeight);
            }


            var newScaleFactor = Mathf.Clamp01(Mathf.Min(screenSize.x / ReferenceResolution.x,
                screenSize.y / ReferenceResolution.y));

            SetScaleFactor(newScaleFactor);
            SetReferencePixelsPerUnit(ReferencePixelsPerUnit);
        }

        private void SetScaleFactor(float scaleFactor)
        {
            if (Mathf.Approximately(scaleFactor, mPrevScaleFactor))
                return;

            mCanvas.scaleFactor = scaleFactor;
            mPrevScaleFactor = scaleFactor;
        }

        private void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
        {
            if (Mathf.Approximately(referencePixelsPerUnit, mPrevReferencePixelsPerUnit))
                return;

            mCanvas.referencePixelsPerUnit = referencePixelsPerUnit;
            mPrevReferencePixelsPerUnit = referencePixelsPerUnit;
        }
    }
}