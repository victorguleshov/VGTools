// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoreColorTintGraphics : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Selectable selectable;
    [SerializeField] private Graphic[] targetGraphics;

    private void Awake()
    {
        if (!selectable) selectable = GetComponent<Selectable>();

        if (selectable.transition == Selectable.Transition.ColorTint)
            foreach (var targetGraphic in targetGraphics)
                targetGraphic.canvasRenderer.SetColor(selectable.colors.normalColor);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectable.interactable && selectable.transition == Selectable.Transition.ColorTint)
            foreach (var targetGraphic in targetGraphics)
                targetGraphic.CrossFadeColor(selectable.colors.pressedColor, selectable.colors.fadeDuration, true,
                    true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (selectable.interactable && selectable.transition == Selectable.Transition.ColorTint)
            foreach (var targetGraphic in targetGraphics)
                targetGraphic.CrossFadeColor(selectable.colors.normalColor, selectable.colors.fadeDuration, true, true);
    }
}