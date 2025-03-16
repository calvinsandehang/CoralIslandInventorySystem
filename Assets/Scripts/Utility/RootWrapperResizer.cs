using UnityEngine;

[ExecuteAlways] // Ensures updates in both Editor & Runtime
public class RootWrapperResizer : MonoBehaviour
{
    [SerializeField] private RectTransform rootWrapper; // Assign in Inspector

    private Vector2 lastResolution = Vector2.zero;

    private void Start()
    {
        AdjustSize();
    }

    private void Update()
    {
        AdjustSize();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
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

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Only apply changes if the resolution actually changes
        if (lastResolution.x != screenWidth || lastResolution.y != screenHeight)
        {
            // 🔥 Force Apply Width & Height dynamically
            rootWrapper.anchorMin = new Vector2(0.5f, 0.5f);
            rootWrapper.anchorMax = new Vector2(0.5f, 0.5f);
            rootWrapper.pivot = new Vector2(0.5f, 0.5f);

            rootWrapper.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth);
            rootWrapper.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, screenHeight);

            // 🚨 Force Unity to refresh RectTransform updates
            rootWrapper.gameObject.SetActive(false);
            rootWrapper.gameObject.SetActive(true);

            lastResolution = new Vector2(screenWidth, screenHeight);

            Debug.Log($"[RootWrapperResizer] Applied Width: {screenWidth}, Height: {screenHeight}");
        }
    }
}
