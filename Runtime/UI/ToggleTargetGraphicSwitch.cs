// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Toggle))]
public class ToggleTargetGraphicSwitch : MonoBehaviour
{
    [SerializeField] private Graphic targetGraphicIsOn;
    [SerializeField] private Graphic targetGraphicIsOff;

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();

        if (!targetGraphicIsOn) targetGraphicIsOn = _toggle.graphic;
        if (!targetGraphicIsOff) targetGraphicIsOff = _toggle.targetGraphic;
    }

    private void Reset()
    {
        Awake();
    }

    private void Start()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
        OnValueChanged(_toggle.isOn);
    }

    private void OnValueChanged(bool value)
    {
        _toggle.targetGraphic = value ? targetGraphicIsOn : targetGraphicIsOff;
    }
}