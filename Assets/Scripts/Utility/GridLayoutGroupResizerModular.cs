using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteAlways]
public class GridLayoutGroupResizerModular : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    [Header("Settings")]
    [SerializeField] private bool adjustCellSize = false;
    [SerializeField] private bool adjustSpacing = false;
    [SerializeField] private bool IsUsingConstraintCount = false; // New boolean

    [Header("Aspect Ratio Configuration")]
    [SerializeField] private List<AspectRatioGridData> gridDataValues = new List<AspectRatioGridData>();

    private float lastAspectRatio = -1f;

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
        if (gridLayoutGroup == null)
        {
            Debug.LogError("[GridLayoutGroupResizer] GridLayoutGroup is not assigned!");
            return;
        }

        float currentAspectRatio = (float)Screen.width / Screen.height;

        // Only update if aspect ratio has changed
        if (Mathf.Abs(currentAspectRatio - lastAspectRatio) > 0.001f)
        {
            Vector2 newCellSize = InterpolateCellSize(currentAspectRatio);
            Vector2 newSpacing = InterpolateSpacing(currentAspectRatio);
            int newConstraintCount = InterpolateConstraintCount(currentAspectRatio);

            if (adjustCellSize)
            {
                gridLayoutGroup.cellSize = newCellSize;
            }

            if (adjustSpacing)
            {
                gridLayoutGroup.spacing = newSpacing;
            }

            if (IsUsingConstraintCount)
            {
                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayoutGroup.constraintCount = newConstraintCount;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(gridLayoutGroup.transform as RectTransform);
            lastAspectRatio = currentAspectRatio;

            Debug.Log($"[GridLayoutGroupResizer] Updated: CellSize: {gridLayoutGroup.cellSize}, Spacing: {gridLayoutGroup.spacing}, ConstraintCount: {gridLayoutGroup.constraintCount}, Aspect Ratio: {currentAspectRatio}");
        }
    }

    private Vector2 InterpolateCellSize(float screenAspectRatio)
    {
        return InterpolateGridData(screenAspectRatio, true);
    }

    private Vector2 InterpolateSpacing(float screenAspectRatio)
    {
        return InterpolateGridData(screenAspectRatio, false);
    }

    private int InterpolateConstraintCount(float screenAspectRatio)
    {
        if (!IsUsingConstraintCount) return gridLayoutGroup.constraintCount;

        if (gridDataValues == null || gridDataValues.Count == 0)
        {
            Debug.LogWarning("[GridLayoutGroupResizer] No aspect ratio values provided.");
            return gridLayoutGroup.constraintCount;
        }

        // Sort list to ensure values are in order
        gridDataValues.Sort((a, b) => a.AspectRatio.CompareTo(b.AspectRatio));

        // If screen aspect ratio matches a predefined one, return the exact constraint count
        foreach (var entry in gridDataValues)
        {
            if (Mathf.Abs(entry.AspectRatio - screenAspectRatio) < 0.01f)
            {
                return entry.ConstraintCount;
            }
        }

        // If screen aspect ratio is below or above the predefined range, return min/max values
        if (screenAspectRatio <= gridDataValues[0].AspectRatio)
        {
            return gridDataValues[0].ConstraintCount;
        }

        if (screenAspectRatio >= gridDataValues[gridDataValues.Count - 1].AspectRatio)
        {
            return gridDataValues[gridDataValues.Count - 1].ConstraintCount;
        }

        // Find two closest values and interpolate
        for (int i = 0; i < gridDataValues.Count - 1; i++)
        {
            if (screenAspectRatio >= gridDataValues[i].AspectRatio && screenAspectRatio <= gridDataValues[i + 1].AspectRatio)
            {
                return Mathf.RoundToInt(Mathf.Lerp(
                    gridDataValues[i].ConstraintCount,
                    gridDataValues[i + 1].ConstraintCount,
                    Mathf.InverseLerp(gridDataValues[i].AspectRatio, gridDataValues[i + 1].AspectRatio, screenAspectRatio)
                ));
            }
        }

        return gridLayoutGroup.constraintCount; // Fallback
    }

    private Vector2 InterpolateGridData(float screenAspectRatio, bool isCellSize)
    {
        if (gridDataValues == null || gridDataValues.Count == 0)
        {
            Debug.LogWarning("[GridLayoutGroupResizer] No aspect ratio values provided.");
            return Vector2.zero;
        }

        // Sort list to ensure values are in order
        gridDataValues.Sort((a, b) => a.AspectRatio.CompareTo(b.AspectRatio));

        // If screen aspect ratio matches a predefined one, return the exact value
        foreach (var entry in gridDataValues)
        {
            if (Mathf.Abs(entry.AspectRatio - screenAspectRatio) < 0.01f)
            {
                return isCellSize ? entry.CellSize : entry.Spacing;
            }
        }

        // If screen aspect ratio is below or above the predefined range, return min/max values
        if (screenAspectRatio <= gridDataValues[0].AspectRatio)
        {
            return isCellSize ? gridDataValues[0].CellSize : gridDataValues[0].Spacing;
        }

        if (screenAspectRatio >= gridDataValues[gridDataValues.Count - 1].AspectRatio)
        {
            return isCellSize ? gridDataValues[gridDataValues.Count - 1].CellSize : gridDataValues[gridDataValues.Count - 1].Spacing;
        }

        // Find two closest values and interpolate
        for (int i = 0; i < gridDataValues.Count - 1; i++)
        {
            if (screenAspectRatio >= gridDataValues[i].AspectRatio && screenAspectRatio <= gridDataValues[i + 1].AspectRatio)
            {
                return isCellSize
                    ? Vector2.Lerp(gridDataValues[i].CellSize, gridDataValues[i + 1].CellSize, Mathf.InverseLerp(gridDataValues[i].AspectRatio, gridDataValues[i + 1].AspectRatio, screenAspectRatio))
                    : Vector2.Lerp(gridDataValues[i].Spacing, gridDataValues[i + 1].Spacing, Mathf.InverseLerp(gridDataValues[i].AspectRatio, gridDataValues[i + 1].AspectRatio, screenAspectRatio));
            }
        }

        return Vector2.zero; // Fallback
    }
}

[System.Serializable]
public class AspectRatioGridData
{
    public float AspectRatio; // Stores predefined aspect ratio values (e.g., 16f / 9f, 21f / 9f)
    public Vector2 CellSize;  // The corresponding cell size for the GridLayout
    public Vector2 Spacing;   // The corresponding spacing (X and Y) for the GridLayout
    public int ConstraintCount; // New: The corresponding column count for the GridLayout constraint
}
