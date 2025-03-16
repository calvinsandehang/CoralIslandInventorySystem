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
    [SerializeField] private Vector2 cellSize_1920_1200 = new Vector2(98f, 98f);

    [Header("Spacing Settings")]
    [SerializeField] private Vector2 spacing_16_9 = new Vector2(20f, 0f);
    [SerializeField] private Vector2 spacing_16_10 = new Vector2(18f, 0f);
    [SerializeField] private Vector2 spacing_21_9 = new Vector2(25f, 0f);
    [SerializeField] private Vector2 spacing_1920_1200 = new Vector2(19f, 0f);

    [Header("Customization")]
    [SerializeField] private bool IsUsingSpaceX = true;
    [SerializeField] private bool IsUsingSpaceY = false;
    [SerializeField] private float spacingMultiplier = 1.0f;

    private int lastAspectIndex = -1;

    private void Start()
    {
        AdjustGridLayout();
    }

    private void Update()
    {
        AdjustGridLayout();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
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
        int closestIndex = AspectRatioUtility.FindClosestAspectRatio(currentAspect, Screen.width, Screen.height);


        Vector2 newCellSize;
        Vector2 newSpacing;

        // Ensure 21:9 & 1920x1200 get correct values instead of incorrect interpolation
        if (closestIndex == 2)
        {
            newCellSize = cellSize_21_9;
            newSpacing = spacing_21_9;
        }
        else if (closestIndex == 3)
        {
            newCellSize = cellSize_1920_1200;
            newSpacing = spacing_1920_1200;
        }
        else
        {
            newCellSize = AspectRatioUtility.InterpolateVector2(
                currentAspect, cellSize_16_10, cellSize_16_9, cellSize_21_9, cellSize_1920_1200
            );

            newSpacing = AspectRatioUtility.InterpolateVector2(
                currentAspect, spacing_16_10, spacing_16_9, spacing_21_9, spacing_1920_1200
            );
        }

        // Apply Spacing Only If Enabled
        if (!IsUsingSpaceX) newSpacing.x = 0;
        if (!IsUsingSpaceY) newSpacing.y = 0;

        // Apply Multiplier to Spacing
        newSpacing *= spacingMultiplier;

        // Only apply changes if the aspect ratio actually changes
        if (closestIndex != lastAspectIndex)
        {
            gridLayout.cellSize = newCellSize;
            gridLayout.spacing = newSpacing;

            // Force UI to Refresh
            LayoutRebuilder.ForceRebuildLayoutImmediate(gridLayout.transform as RectTransform);

            lastAspectIndex = closestIndex;

            Debug.Log($"[GridLayoutResizer] Applied CellSize: {newCellSize.x}x{newCellSize.y}, Spacing: {newSpacing.x}x{newSpacing.y}, Aspect Ratio: {currentAspect}");
        }
    }
}
