using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // Ensures updates in both Editor & Runtime
public class GridLayoutResizer : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayout; // Assign in Inspector

    [Header("Cell Size Settings")]
    [SerializeField] private Vector2 cellSize_16_9 = new Vector2(100f, 100f);
    [SerializeField] private Vector2 cellSize_16_10 = new Vector2(95f, 95f);
    [SerializeField] private Vector2 cellSize_21_9 = new Vector2(120f, 120f);

    [Header("Spacing Settings")]
    [SerializeField] private Vector2 spacing_16_9 = new Vector2(20f, 0f);
    [SerializeField] private Vector2 spacing_16_10 = new Vector2(18f, 0f);
    [SerializeField] private Vector2 spacing_21_9 = new Vector2(25f, 0f);

    [Header("Customization")]
    [SerializeField] private bool IsUsingSpaceX = true;
    [SerializeField] private bool IsUsingSpaceY = false;
    [SerializeField] private float spacingMultiplier = 1.0f; // Adjusts the spacing scaling

    private float[] predefinedAspectRatios = { 16f / 10f, 16f / 9f, 21f / 9f };
    private Vector2[] cellSizes;
    private Vector2[] spacings;
    private int lastIndex = -1;

    private void Start()
    {
        cellSizes = new Vector2[] { cellSize_16_10, cellSize_16_9, cellSize_21_9 };
        spacings = new Vector2[] { spacing_16_10, spacing_16_9, spacing_21_9 };
        AdjustGridLayout();
    }

    private void Update()
    {
        AdjustGridLayout();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        cellSizes = new Vector2[] { cellSize_16_10, cellSize_16_9, cellSize_21_9 };
        spacings = new Vector2[] { spacing_16_10, spacing_16_9, spacing_21_9 };
        AdjustGridLayout();
    }
    #endif

    private void AdjustGridLayout()
    {
        if (gridLayout == null)
        {
            Debug.LogError("[GridLayoutResizer] GridLayoutGroup is NOT assigned!");
            return;
        }

        float currentAspect = (float)Screen.width / Screen.height;
        int closestIndex = FindClosestAspectRatio(currentAspect);

        // Only apply changes if the aspect ratio actually changes
        if (closestIndex != lastIndex)
        {
            Vector2 newCellSize = cellSizes[closestIndex];
            Vector2 newSpacing = spacings[closestIndex];

            // 🔥 Apply Spacing Only If Enabled
            if (!IsUsingSpaceX) newSpacing.x = 0;
            if (!IsUsingSpaceY) newSpacing.y = 0;

            // Apply Multiplier to Spacing
            newSpacing *= spacingMultiplier;

            // 🔥 Apply to Grid Layout
            gridLayout.cellSize = newCellSize;
            gridLayout.spacing = newSpacing;

            lastIndex = closestIndex;

            Debug.Log($"✅ [GridLayoutResizer] Applied CellSize: {newCellSize.x}x{newCellSize.y}, Spacing: {newSpacing.x}x{newSpacing.y}, Aspect Ratio: {predefinedAspectRatios[closestIndex]}");
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
