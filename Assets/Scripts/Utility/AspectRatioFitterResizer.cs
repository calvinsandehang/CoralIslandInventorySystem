using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteAlways]
public class AspectRatioFitterResizer : MonoBehaviour
{
    [SerializeField] private AspectRatioFitter aspectRatioFitter;

    [Header("Aspect Ratio Configuration")]
    [SerializeField] private List<AspectRatioValue> aspectRatioValues = new List<AspectRatioValue>();

    private float lastAspectRatio = -1f;

    private void Start()
    {
        AdjustAspectRatio();
    }

    private void Update()
    {
        AdjustAspectRatio();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        AdjustAspectRatio();
    }
#endif

    private void AdjustAspectRatio()
    {
        if (aspectRatioFitter == null)
        {
            Debug.LogError("[AspectRatioFitterResizer] AspectRatioFitter is NOT assigned!");
            return;
        }

        float currentAspectRatio = (float)Screen.width / Screen.height;

        // Only update if aspect ratio has changed
        if (Mathf.Abs(currentAspectRatio - lastAspectRatio) > 0.001f)
        {
            float newAspectRatio = InterpolateAspectRatio(currentAspectRatio);

            // Apply aspect ratio only for FitInParent or WidthControlsHeight modes
            if (aspectRatioFitter.aspectMode == AspectRatioFitter.AspectMode.FitInParent ||
                aspectRatioFitter.aspectMode == AspectRatioFitter.AspectMode.WidthControlsHeight)
            {
                aspectRatioFitter.aspectRatio = newAspectRatio;
                Debug.Log($"[AspectRatioFitterResizer] Applied Aspect Ratio: {newAspectRatio} for Screen Aspect: {currentAspectRatio}, Mode: {aspectRatioFitter.aspectMode}");
            }

            lastAspectRatio = currentAspectRatio;
        }
    }

    private float InterpolateAspectRatio(float screenAspectRatio)
    {
        if (aspectRatioValues == null || aspectRatioValues.Count == 0)
        {
            Debug.LogWarning("[AspectRatioFitterResizer] No aspect ratio data provided.");
            return 1f; // Default aspect ratio
        }

        // Sort list to ensure values are in order
        aspectRatioValues.Sort((a, b) => a.AspectRatio.CompareTo(b.AspectRatio));

        // If screen aspect ratio is below or equal to the lowest Value, return the lowest
        if (screenAspectRatio <= aspectRatioValues[0].AspectRatio)
        {
            return aspectRatioValues[0].Value;
        }

        // If screen aspect ratio is above or equal to the highest Value, return the highest
        if (screenAspectRatio >= aspectRatioValues[aspectRatioValues.Count - 1].AspectRatio)
        {
            return aspectRatioValues[aspectRatioValues.Count - 1].Value;
        }

        // Find two closest values and interpolate
        for (int i = 0; i < aspectRatioValues.Count - 1; i++)
        {
            if (screenAspectRatio >= aspectRatioValues[i].AspectRatio && screenAspectRatio <= aspectRatioValues[i + 1].AspectRatio)
            {
                return Mathf.Lerp(
                    aspectRatioValues[i].Value,
                    aspectRatioValues[i + 1].Value,
                    Mathf.InverseLerp(aspectRatioValues[i].AspectRatio, aspectRatioValues[i + 1].AspectRatio, screenAspectRatio)
                );
            }
        }

        return 1f; // Fallback
    }
}

[System.Serializable]
public class AspectRatioValue
{
    public float AspectRatio; // Stores predefined aspect ratio values (e.g., 16f / 9f, 21f / 9f)
    public float Value; // The corresponding aspect ratio Value for the AspectRatioFitter
}
