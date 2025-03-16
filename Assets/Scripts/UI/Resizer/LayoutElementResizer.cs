using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LayoutElementResizer : MonoBehaviour
{
    [SerializeField] private LayoutElement layoutElement;

    [Header("Width Settings")]
    [SerializeField] private bool IsUsingMinWidth = true;
    [SerializeField] private float minWidth_16_10 = 250f;
    [SerializeField] private float minWidth_16_9 = 255f;
    [SerializeField] private float minWidth_21_9 = 270f;
    [SerializeField] private float minWidth_1920_1200 = 260f;

    [Header("Height Settings")]
    [SerializeField] private bool IsUsingMinHeight = true;
    [SerializeField] private float minHeight_16_10 = 100f;
    [SerializeField] private float minHeight_16_9 = 110f;
    [SerializeField] private float minHeight_21_9 = 120f;
    [SerializeField] private float minHeight_1920_1200 = 105f;

    private int lastAspectIndex = -1;
    private float lastAspectRatio = -1f;

    private void Start()
    {
        AdjustLayout();
    }

    private void Update()
    {
        AdjustLayout();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        AdjustLayout();
    }
#endif

    private void AdjustLayout()
    {
        if (layoutElement == null)
        {
            Debug.LogError("[LayoutElementResizer] LayoutElement is not assigned!");
            return;
        }

        float currentAspect = (float)Screen.width / Screen.height;
        int closestIndex = AspectRatioUtility.FindClosestAspectRatio(currentAspect, Screen.width, Screen.height);

        if (AspectRatioUtility.IsPredefinedAspectRatio(currentAspect, Screen.width, Screen.height))
        {
            // Use exact predefined values
            switch (closestIndex)
            {
                case 0: // 16:10
                    layoutElement.minWidth = minWidth_16_10;
                    layoutElement.minHeight = minHeight_16_10;
                    break;
                case 1: // 16:9
                    layoutElement.minWidth = minWidth_16_9;
                    layoutElement.minHeight = minHeight_16_9;
                    break;
                case 2: // 21:9
                    layoutElement.minWidth = minWidth_21_9;
                    layoutElement.minHeight = minHeight_21_9;
                    break;
                case 3: // 1920x1200
                    layoutElement.minWidth = minWidth_1920_1200;
                    layoutElement.minHeight = minHeight_1920_1200;
                    break;
            }
        }
        else
        {
            // Apply interpolation when the aspect ratio is not predefined
            if (IsUsingMinWidth)
            {
                layoutElement.minWidth = AspectRatioUtility.InterpolateValue(
                    currentAspect, minWidth_16_10, minWidth_16_9, minWidth_21_9, minWidth_1920_1200
                );
            }

            if (IsUsingMinHeight)
            {
                layoutElement.minHeight = AspectRatioUtility.InterpolateValue(
                    currentAspect, minHeight_16_10, minHeight_16_9, minHeight_21_9, minHeight_1920_1200
                );
            }
        }

        if (closestIndex != lastAspectIndex || Mathf.Abs(currentAspect - lastAspectRatio) > 0.001f)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutElement.transform as RectTransform);
            lastAspectRatio = currentAspect;
            lastAspectIndex = closestIndex;

            Debug.Log($"[LayoutElementResizer] Updated: minWidth: {layoutElement.minWidth}, minHeight: {layoutElement.minHeight}, Aspect Ratio: {currentAspect}");
        }
    }
}
