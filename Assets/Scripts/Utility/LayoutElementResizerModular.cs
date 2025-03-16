using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteAlways]
public class LayoutElementResizerModular : MonoBehaviour
{
    [SerializeField] private LayoutElement layoutElement;

    [Header("Settings")]
    [SerializeField] private bool adjustMinWidth = false;
    [SerializeField] private bool adjustMinHeight = false;

    [Header("Aspect Ratio Configuration")]
    [SerializeField] private List<AspectRatioValue> minWidthValues = new List<AspectRatioValue>();
    [SerializeField] private List<AspectRatioValue> minHeightValues = new List<AspectRatioValue>();

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
            Debug.LogError("[LayoutElementResizerModular] LayoutElement is not assigned!");
            return;
        }

        float currentAspectRatio = (float)Screen.width / Screen.height;

        // Only update if aspect ratio has changed
        if (Mathf.Abs(currentAspectRatio - lastAspectRatio) > 0.001f)
        {
            if (adjustMinWidth)
            {
                layoutElement.minWidth = InterpolateAspectRatioValue(currentAspectRatio, minWidthValues);
            }

            if (adjustMinHeight)
            {
                layoutElement.minHeight = InterpolateAspectRatioValue(currentAspectRatio, minHeightValues);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutElement.transform as RectTransform);
            lastAspectRatio = currentAspectRatio;

            Debug.Log($"[LayoutElementResizerModular] Applied MinWidth: {layoutElement.minWidth}, MinHeight: {layoutElement.minHeight}, Aspect Ratio: {currentAspectRatio}");
        }
    }

    private float InterpolateAspectRatioValue(float screenAspectRatio, List<AspectRatioValue> values)
    {
        if (values == null || values.Count == 0)
        {
            Debug.LogWarning("[LayoutElementResizerModular] No aspect ratio values provided.");
            return 0f; // Default Value
        }

        // Sort list to ensure values are in order
        values.Sort((a, b) => a.AspectRatio.CompareTo(b.AspectRatio));

        // If screen aspect ratio is exactly in the predefined list, return the exact Value
        foreach (var entry in values)
        {
            if (Mathf.Abs(entry.AspectRatio - screenAspectRatio) < 0.01f)
            {
                return entry.Value;
            }
        }

        // If screen aspect ratio is below or above the predefined range, return min/max values
        if (screenAspectRatio <= values[0].AspectRatio)
        {
            return values[0].Value;
        }

        if (screenAspectRatio >= values[values.Count - 1].AspectRatio)
        {
            return values[values.Count - 1].Value;
        }

        // Find two closest values and interpolate
        for (int i = 0; i < values.Count - 1; i++)
        {
            if (screenAspectRatio >= values[i].AspectRatio && screenAspectRatio <= values[i + 1].AspectRatio)
            {
                return Mathf.Lerp(
                    values[i].Value,
                    values[i + 1].Value,
                    Mathf.InverseLerp(values[i].AspectRatio, values[i + 1].AspectRatio, screenAspectRatio)
                );
            }
        }

        return values[0].Value; // Fallback
    }
}