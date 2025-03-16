using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AspectRatioFitterResizer : MonoBehaviour
{
    [SerializeField] private AspectRatioFitter aspectRatioFitter;

    // Predefined screen width and aspect ratio values
    private float screenWidth_1920 = 1920f;
    private float aspectRatio_1920 = 5.37f;

    private float screenWidth_2560 = 2560f;
    private float aspectRatio_2560 = 7.29f;

    private float screenWidth_1600 = 1600f;
    private float aspectRatio_1600 = 4.56f;

    private float lastScreenWidth = -1f;

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

        // Only update when the mode is FitInParent
        if (aspectRatioFitter.aspectMode != AspectRatioFitter.AspectMode.FitInParent)
        {
            return;
        }

        float currentScreenWidth = Screen.width;

        // Only update if screen width has changed to avoid unnecessary calculations
        if (Mathf.Abs(currentScreenWidth - lastScreenWidth) > 1f)
        {
            float newAspectRatio = InterpolateAspectRatio(currentScreenWidth);

            // Apply new aspect ratio
            aspectRatioFitter.aspectRatio = newAspectRatio;

            lastScreenWidth = currentScreenWidth;

            Debug.Log($"[AspectRatioFitterResizer] Applied Aspect Ratio: {newAspectRatio} for Screen Width: {currentScreenWidth}");
        }
    }

    private float InterpolateAspectRatio(float screenWidth)
    {
        if (screenWidth <= screenWidth_1600)
        {
            return aspectRatio_1600;
        }
        else if (screenWidth >= screenWidth_2560)
        {
            return aspectRatio_2560;
        }
        else if (screenWidth <= screenWidth_1920)
        {
            return Mathf.Lerp(aspectRatio_1600, aspectRatio_1920, Mathf.InverseLerp(screenWidth_1600, screenWidth_1920, screenWidth));
        }
        else
        {
            return Mathf.Lerp(aspectRatio_1920, aspectRatio_2560, Mathf.InverseLerp(screenWidth_1920, screenWidth_2560, screenWidth));
        }
    }
}
