using System.Collections.Generic;
using UnityEngine;
using VG.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VG.UI
{
    public class SegmentedSlider : MonoBehaviour
    {
        [SerializeField] [Min(1)] private int segmentCount;
        [SerializeField] private List<RectTransform> segments;
        [SerializeField] private RectTransform fillRect;
        [SerializeField] [Range(0f, 1f)] private float value;

        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                if (fillRect) SetSliderPosition(value);
            }
        }

        public int SegmentsCount
        {
            get => segmentCount;
            set
            {
                segmentCount = value;

                if (segments.Count > 0)
                    segments.SetViews(segmentCount - 1, (v, i) =>
                    {
                        v.anchorMax = new Vector2((i + 1f) / segmentCount, v.anchorMax.y);
                        v.anchorMin = new Vector2((i + 1f) / segmentCount, v.anchorMin.y);
                    });
            }
        }

        private void SetSliderPosition(float value)
        {
            fillRect.anchorMax = new Vector2(value, fillRect.anchorMax.y);
            fillRect.anchorMin = new Vector2(0.0f, fillRect.anchorMin.y);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SegmentedSlider))]
    internal class SegmentedSliderEditor : Editor
    {
        private SerializedProperty fillRectProperty;
        private SerializedProperty segmentCountProperty;
        private SerializedProperty segmentsProperty;
        private SerializedProperty valueProperty;

        private void OnEnable()
        {
            segmentCountProperty = serializedObject.FindProperty("segmentCount");
            segmentsProperty = serializedObject.FindProperty("segments");
            fillRectProperty = serializedObject.FindProperty("fillRect");
            valueProperty = serializedObject.FindProperty("value");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(segmentCountProperty);
            EditorGUILayout.PropertyField(segmentsProperty);
            EditorGUILayout.PropertyField(fillRectProperty);
            EditorGUILayout.PropertyField(valueProperty);

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();

                ((SegmentedSlider)target).SegmentsCount = segmentCountProperty.intValue;
                ((SegmentedSlider)target).Value = valueProperty.floatValue;
            }
        }
    }
#endif
}