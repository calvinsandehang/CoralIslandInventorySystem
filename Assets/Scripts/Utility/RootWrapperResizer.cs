using UnityEngine;

[ExecuteAlways] // Ensures updates in both Editor & Runtime
public class RootWrapperResizer : MonoBehaviour
{
    [SerializeField] private RectTransform rootWrapper; // Assign in Inspector

    [Header("Size Settings")]
    [SerializeField] private Vector2 resolution_16_9 = new Vector2(1920f, 1080f);
    [SerializeField] private Vector2 resolution_16_10 = new Vector2(1600f, 1000f);
    [SerializeField] private Vector2 resolution_21_9 = new Vector2(2560f, 1080f);

    private float[] predefinedAspectRatios = { 16f / 10f, 16f / 9f, 21f / 9f };
    private Vector2[] resolutions;
    private int lastIndex = -1;

    private void Start()
    {
        resolutions = new Vector2[] { resolution_16_10, resolution_16_9, resolution_21_9 };
        AdjustSize();
    }

    private void Update()
    {
        AdjustSize();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        resolutions = new Vector2[] { resolution_16_10, resolution_16_9, resolution_21_9 };
        AdjustSize();
    }
#endif

    private void AdjustSize()
    {
        if (rootWrapper == null)
        {
            Debug.LogError("[RootWrapperResizer] RootWrapper is NOT assigned!");
            return;
        }

        float currentAspect = (float)Screen.width / Screen.height;
        int closestIndex = FindClosestAspectRatio(currentAspect);

        // Only apply changes if the aspect ratio actually changes
        if (closestIndex != lastIndex)
        {
            Vector2 newSize = resolutions[closestIndex];

            // 🔥 Force Apply Width & Height
            rootWrapper.anchorMin = new Vector2(0.5f, 0.5f);
            rootWrapper.anchorMax = new Vector2(0.5f, 0.5f);
            rootWrapper.pivot = new Vector2(0.5f, 0.5f);

            rootWrapper.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSize.x);
            rootWrapper.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSize.y);

            // 🚨 Force Unity to refresh RectTransform updates
            rootWrapper.gameObject.SetActive(false);
            rootWrapper.gameObject.SetActive(true);

            lastIndex = closestIndex;

            Debug.Log($"✅ [RootWrapperResizer] Applied Width: {newSize.x}, Height: {newSize.y}, Closest Aspect Ratio: {predefinedAspectRatios[closestIndex]}");
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
