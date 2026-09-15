using UnityEngine;
using UnityEngine.UI;

public class UICircularLayoutGroup : LayoutGroup
{
    [Header("„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ")]
    [SerializeField] float childSize = 70f;
    [SerializeField] float containerPadding = 1f;

    private enum Axis { X = 0, Y = 1 }

    private float angle, containerRadius, radiusPadded;


    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        CalculateLayout(Axis.X);
    }
    public override void CalculateLayoutInputVertical()
    {
        CalculateLayout(Axis.Y);
    }

    public override void SetLayoutHorizontal()
    {
        SetLayout(Axis.X);
    }
    public override void SetLayoutVertical()
    {
        SetLayout(Axis.Y);
    }

    private void CalculateLayout(Axis axis)
    {
        angle = (360f / rectChildren.Count) * Mathf.Deg2Rad;

        float containerSize;
        if (axis == Axis.X)
            containerSize = rectTransform.rect.width;
        else
            containerSize = rectTransform.rect.height;

        containerRadius = containerSize / 2f;
        radiusPadded = containerRadius - containerPadding - childSize;
    }

    private void SetLayout(Axis axis)
    {
        for (int i = 0; i < rectChildren.Count; i++)
        {
            RectTransform child = rectChildren[i];

            float angularPos;
            if (axis == Axis.X)
                angularPos = Mathf.Cos(i * angle - Mathf.PI / 2f);
            else
                angularPos = Mathf.Sin(i * angle - Mathf.PI / 2f);

            float paddedPos = radiusPadded * angularPos + containerRadius - childSize;

            SetChildAlongAxis(child, (int)axis, paddedPos, childSize * 2f);
        }
    }
}
