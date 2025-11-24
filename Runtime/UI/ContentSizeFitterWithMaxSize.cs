// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.UI
{
    [AddComponentMenu("Layout/Content Size Fitter With Max Size", 141)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class ContentSizeFitterWithMaxSize : UIBehaviour, ILayoutSelfController
    {
        public enum FitMode
        {
            Unconstrained,
            MinSize,
            PreferredSize,
            Flexible
        }

        [SerializeField] protected FitMode m_HorizontalFit = FitMode.Unconstrained;
        [SerializeField] protected FitMode m_VerticalFit = FitMode.Unconstrained;
        [SerializeField] protected float m_MinWidth = -1f;
        [SerializeField] protected float m_MinHeight = -1f;
        [SerializeField] protected float m_MaxWidth = -1f;
        [SerializeField] protected float m_MaxHeight = -1f;

        [NonSerialized] private RectTransform m_Rect;

        private DrivenRectTransformTracker m_Tracker;

        protected ContentSizeFitterWithMaxSize()
        {
        }

        public FitMode horizontalFit
        {
            get => m_HorizontalFit;
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_HorizontalFit, value)) SetDirty();
            }
        }

        public FitMode verticalFit
        {
            get => m_VerticalFit;
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_VerticalFit, value)) SetDirty();
            }
        }

        public float minWidth
        {
            get => m_MinWidth;
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_MinWidth, value)) SetDirty();
            }
        }

        public float minHeight
        {
            get => m_MinHeight;
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_MinHeight, value)) SetDirty();
            }
        }

        public float maxWidth
        {
            get => m_MaxWidth;
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_MaxWidth, value)) SetDirty();
            }
        }

        public float maxHeight
        {
            get => m_MaxHeight;
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_MaxHeight, value)) SetDirty();
            }
        }

        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }
#endif

        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }

        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }

        private void HandleSelfFittingAlongAxis(int axis)
        {
            var fitting = axis == 0 ? horizontalFit : verticalFit;
            if (fitting == FitMode.Unconstrained)
            {
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
                return;
            }

            m_Tracker.Add(this, rectTransform,
                axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY);

            float size;
            switch (fitting)
            {
                case FitMode.MinSize:
                    size = LayoutUtility.GetMinSize(m_Rect, axis);
                    break;

                case FitMode.PreferredSize:
                    size = LayoutUtility.GetPreferredSize(m_Rect, axis);
                    break;
                case FitMode.Flexible:
                    size = LayoutUtility.GetFlexibleSize(m_Rect, axis);

                    if (size <= 0 && rectTransform.parent is RectTransform parentRect)
                    {
                        var parentSize = axis == 0 ? parentRect.rect.width : parentRect.rect.height;

                        if (parentRect.TryGetComponent<LayoutGroup>(out var layoutGroup))
                        {
                            var padding = layoutGroup.padding;
                            parentSize -= axis == 0 ? padding.left + padding.right : padding.top + padding.bottom;
                        }

                        size = parentSize;
                    }

                    break;

                default:
                    size = 0;
                    break;
            }

            // Применяем ограничения на размер
            size = axis == 0
                ? Mathf.Clamp(size, minWidth >= 0 ? minWidth : size, maxWidth >= 0 ? maxWidth : size)
                : Mathf.Clamp(size, minHeight >= 0 ? minHeight : size, maxHeight >= 0 ? maxHeight : size);

            rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, size);
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }

    internal static class SetPropertyUtility
    {
        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;
            return true;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ContentSizeFitterWithMaxSize), true)]
    [CanEditMultipleObjects]
    public class ContentSizeFitterWithMaxSizeEditor : Editor
    {
        private SerializedProperty m_HorizontalFit;
        private SerializedProperty m_MaxHeight;
        private SerializedProperty m_MaxWidth;
        private SerializedProperty m_MinHeight;
        private SerializedProperty m_MinWidth;
        private SerializedProperty m_VerticalFit;

        protected virtual void OnEnable()
        {
            m_HorizontalFit = serializedObject.FindProperty("m_HorizontalFit");
            m_VerticalFit = serializedObject.FindProperty("m_VerticalFit");
            m_MinWidth = serializedObject.FindProperty("m_MinWidth");
            m_MinHeight = serializedObject.FindProperty("m_MinHeight");
            m_MaxWidth = serializedObject.FindProperty("m_MaxWidth");
            m_MaxHeight = serializedObject.FindProperty("m_MaxHeight");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_HorizontalFit, new GUIContent("Horizontal Fit"));
            EditorGUILayout.PropertyField(m_VerticalFit, new GUIContent("Vertical Fit"));

            EditorGUILayout.Space();

            LayoutElementField(m_MinWidth, 0);
            LayoutElementField(m_MinHeight, 0);
            LayoutElementField(m_MaxWidth, t => t.rect.width);
            LayoutElementField(m_MaxHeight, t => t.rect.height);

            serializedObject.ApplyModifiedProperties();
        }

        private void LayoutElementField(SerializedProperty property, float defaultValue)
        {
            LayoutElementField(property, _ => defaultValue);
        }

        private void LayoutElementField(SerializedProperty property, Func<RectTransform, float> defaultValue)
        {
            var position = EditorGUILayout.GetControlRect();

            // Label
            var label = EditorGUI.BeginProperty(position, null, property);

            // Rects
            var fieldPosition = EditorGUI.PrefixLabel(position, label);

            var toggleRect = fieldPosition;
            toggleRect.width = 16;

            var floatFieldRect = fieldPosition;
            floatFieldRect.xMin += 16;

            // Checkbox
            EditorGUI.BeginChangeCheck();
            var enabled = EditorGUI.ToggleLeft(toggleRect, GUIContent.none, property.floatValue >= 0);
            if (EditorGUI.EndChangeCheck())
                // This could be made better to set all of the targets to their initial width, but mimizing code change for now
                property.floatValue =
                    enabled ? defaultValue((target as ContentSizeFitterWithMaxSize).transform as RectTransform) : -1;

            if (!property.hasMultipleDifferentValues && property.floatValue >= 0)
            {
                // Float field
                EditorGUIUtility.labelWidth = 4; // Small invisible label area for drag zone functionality
                EditorGUI.BeginChangeCheck();
                var newValue = EditorGUI.FloatField(floatFieldRect, new GUIContent(" "), property.floatValue);
                if (EditorGUI.EndChangeCheck()) property.floatValue = Mathf.Max(0, newValue);

                EditorGUIUtility.labelWidth = 0;
            }

            EditorGUI.EndProperty();
        }
    }

#endif
}