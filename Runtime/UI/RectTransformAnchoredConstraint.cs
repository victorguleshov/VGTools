// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using UnityEngine;

[ExecuteAlways]
public class RectTransformAnchoredConstraint : MonoBehaviour
{
    public enum AnchorPoint
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    [Header("Constraints")] public RectTransform target;
    public Vector2 offset;

    [Header("Anchor Points")] public AnchorPoint targetAnchor = AnchorPoint.MiddleCenter;

    [Header("Axes")] public bool constraintX = true;
    public bool constraintY = true;

    public bool updateOnlyWhenTargetChanges;
    private Vector2 _lastPivot;
    private Vector2 _lastTargetPosition;
    private Vector2 _lastTargetSize;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (target == null || _rectTransform == null)
            return;

        var needUpdate = !updateOnlyWhenTargetChanges ||
                         _lastTargetPosition != target.anchoredPosition ||
                         _lastTargetSize != target.rect.size ||
                         _lastPivot != _rectTransform.pivot;

        if (needUpdate)
        {
            ApplyConstraints();

            _lastTargetPosition = target.anchoredPosition;
            _lastTargetSize = target.rect.size;
            _lastPivot = _rectTransform.pivot;
        }
    }

    private void OnEnable()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        if (_rectTransform != null)
            _lastPivot = _rectTransform.pivot;

        if (target != null)
        {
            _lastTargetPosition = target.anchoredPosition;
            _lastTargetSize = target.rect.size;
        }
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying || target == null || !enabled)
            return;

        _rectTransform = GetComponent<RectTransform>();
        ApplyConstraints();
    }
#endif

    private Vector2 GetAnchorPositionOffset(RectTransform rt, AnchorPoint anchorPoint)
    {
        var size = rt.rect.size;
        var pivot = rt.pivot;

        switch (anchorPoint)
        {
            case AnchorPoint.TopLeft:
                return new Vector2(-size.x * pivot.x, size.y * (1 - pivot.y));
            case AnchorPoint.TopCenter:
                return new Vector2(size.x * (0.5f - pivot.x), size.y * (1 - pivot.y));
            case AnchorPoint.TopRight:
                return new Vector2(size.x * (1 - pivot.x), size.y * (1 - pivot.y));
            case AnchorPoint.MiddleLeft:
                return new Vector2(-size.x * pivot.x, size.y * (0.5f - pivot.y));
            case AnchorPoint.MiddleCenter:
                return new Vector2(size.x * (0.5f - pivot.x), size.y * (0.5f - pivot.y));
            case AnchorPoint.MiddleRight:
                return new Vector2(size.x * (1 - pivot.x), size.y * (0.5f - pivot.y));
            case AnchorPoint.BottomLeft:
                return new Vector2(-size.x * pivot.x, -size.y * pivot.y);
            case AnchorPoint.BottomCenter:
                return new Vector2(size.x * (0.5f - pivot.x), -size.y * pivot.y);
            case AnchorPoint.BottomRight:
                return new Vector2(size.x * (1 - pivot.x), -size.y * pivot.y);
            default:
                return Vector2.zero;
        }
    }

    private void ApplyConstraints()
    {
        // Рассчитываем точку привязки цели
        var targetAnchorOffset = GetAnchorPositionOffset(target, targetAnchor);

        // Получаем позицию целевой точки
        var targetPosition = target.anchoredPosition + targetAnchorOffset;

        var desiredPosition = targetPosition + offset;

        var newPosition = _rectTransform.anchoredPosition;

        if (constraintX)
            newPosition.x = desiredPosition.x;

        if (constraintY)
            newPosition.y = desiredPosition.y;

        _rectTransform.anchoredPosition = newPosition;
    }
}