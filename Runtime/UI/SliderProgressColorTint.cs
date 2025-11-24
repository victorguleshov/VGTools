// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[ExecuteAlways]
public class SliderProgressColorTint : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Slider slider;
    [SerializeField] private Graphic target;

    [SerializeField] private ProgressColor[] tints = Array.Empty<ProgressColor>();

    [SerializeField] private float fadeDuration = 0.1f;

    private void Awake()
    {
        if (!slider) slider = GetComponent<Slider>();
        if (!target) target = slider.fillRect.GetComponent<Graphic>();

        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        OnValueChanged(slider.value);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        OnDestroy();

        Awake();
    }
#endif

    private void OnValueChanged(float fillValue)
    {
        var targetColor = Color.white;
        foreach (var item in tints)
            if (fillValue >= item.progress)
                targetColor = item.color;

        var currentColor = target.canvasRenderer.GetColor();
        if (currentColor == targetColor) return;

        if (target.gameObject.activeInHierarchy)
            target.CrossFadeColor(targetColor, fadeDuration, true, true);
        else
            target.canvasRenderer.SetColor(targetColor);
    }

    [Serializable]
    public class ProgressColor
    {
        public Color color = Color.white;
        [Range(0.0f, 1.0f)] public float progress;
    }
}