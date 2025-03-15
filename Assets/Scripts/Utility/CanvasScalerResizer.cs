using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // Ensures updates in both Editor & Runtime
public class CanvasScalerResizer : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler; // Assign in Inspector

    [Header("Reference Resolutions")]
    [SerializeField] private Vector2 resolution_16_9 = new Vector2(1920f, 1080f);
    [SerializeField] private Vector2 resolution_16_10 = new Vector2(1600f, 1000f);
    [SerializeField] private Vector2 resolution_21_9 = new Vector2(2560f, 1080f);

    private float[] predefinedAspectRatios = { 16f / 10f, 16f / 9f, 21f / 9f };
    private Vector2[] resolutions;
    private int lastIndex = -1;

    private void Start()
    {
        resolutions = new Vector2[] { resolution_16_10, resolution_16_9, resolution_21_9 };
        AdjustCanvasScaler();
    }

    private void Update()
    {
        AdjustCanvasScaler();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        resolutions = new Vector2[] { resolution_16_10, resolution_16_9, resolution_21_9 };
        AdjustCanvasScaler();
    }
#endif

    private void AdjustCanvasScaler()
    {
        if (canvasScaler == null)
        {
            Debug.LogError("[CanvasScalerResizer] CanvasScaler is NOT assigned!");
            return;
        }

        float currentAspect = (float)Screen.width / Screen.height;
        int closestIndex = FindClosestAspectRatio(currentAspect);

        // Only apply changes if the aspect ratio actually changes
        if (closestIndex != lastIndex)
        {
            Vector2 newResolution = resolutions[closestIndex];

            // 🚨 Force Apply New Resolution 🚨
            canvasScaler.referenceResolution = newResolution;
            canvasScaler.enabled = false;  // 🔥 Force Unity to Refresh UI Scaling
            canvasScaler.enabled = true;   // 🔥 Reactivates to Apply New Resolution

            lastIndex = closestIndex;

            Debug.Log($"✅ [CanvasScalerResizer] Applied Resolution: {newResolution.x}x{newResolution.y}, Closest Aspect Ratio: {predefinedAspectRatios[closestIndex]}");
        }
    }

    private int FindClosestAspectRatio(float aspect)
    {
        int closestIndex = 0;
        float closestDifference = Mathf.Abs(predefinedAspectRatios[0] - aspect);

        for (int i = 1; i < predefinedAspectRatios.Length; i++)
        {
            float difference = Mathf.Abs(predefinedAspectRatios[i] - aspect);
            if (difference < closestDifference)
            {
                closestIndex = i;
                closestDifference = difference;
            }
        }
        return closestIndex;
    }
}
