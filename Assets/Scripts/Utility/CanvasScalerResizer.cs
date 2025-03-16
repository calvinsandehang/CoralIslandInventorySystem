using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // Ensures updates in both Editor & Runtime
public class CanvasScalerResizer : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler; // Assign in Inspector

    private Vector2 lastResolution = Vector2.zero;

    private void Start()
    {
        AdjustCanvasScaler();
    }

    private void Update()
    {
        AdjustCanvasScaler();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
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

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Only apply changes if resolution actually changes
        if (lastResolution.x != screenWidth || lastResolution.y != screenHeight)
        {
            canvasScaler.referenceResolution = new Vector2(screenWidth, screenHeight);
            canvasScaler.enabled = false;  // Forces Unity to Refresh UI Scaling
            canvasScaler.enabled = true;

            lastResolution = new Vector2(screenWidth, screenHeight);
            Debug.Log($"[CanvasScalerResizer] Applied Resolution: {screenWidth}x{screenHeight}");
        }
    }
}
