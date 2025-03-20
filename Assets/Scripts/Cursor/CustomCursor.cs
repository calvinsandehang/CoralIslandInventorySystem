using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Image cursorImage; // UI Image assigned in Inspector
    private RectTransform cursorRect;

    [Header("Cursor Sizes for Aspect Ratios")]
    [SerializeField] private Vector2 size1_6 = new Vector2(40, 40);
    [SerializeField] private Vector2 size1_7 = new Vector2(50, 50);
    [SerializeField] private Vector2 size2_3 = new Vector2(70, 70);

    [Header("Cursor Offsets for Aspect Ratios")]
    [SerializeField] private Vector2 offset1_6 = Vector2.zero;
    [SerializeField] private Vector2 offset1_7 = Vector2.zero;
    [SerializeField] private Vector2 offset2_3 = Vector2.zero;

    private void Start()
    {
        Cursor.visible = false; // Hide system cursor
        cursorRect = cursorImage.GetComponent<RectTransform>();
        AdjustCursorSizeAndOffset();
    }

    private void Update()
    {
        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        Vector2 cursorPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            cursorRect.parent as RectTransform, // Get parent RectTransform (Canvas)
            Input.mousePosition,
            null,
            out cursorPosition
        );

        float aspectRatio = (float)Screen.width / Screen.height;
        Vector2 currentOffset = InterpolateOffset(aspectRatio);

        cursorRect.anchoredPosition = cursorPosition + currentOffset;
    }

    private void AdjustCursorSizeAndOffset()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        cursorRect.sizeDelta = InterpolateSize(aspectRatio);
    }

    private Vector2 InterpolateSize(float aspectRatio)
    {
        float aspect1_6 = 1.6f;
        float aspect1_7 = 1.7f;
        float aspect2_3 = 2.3f;

        if (aspectRatio <= aspect1_6)
            return size1_6;
        else if (aspectRatio >= aspect2_3)
            return size2_3;
        else if (aspectRatio > aspect1_6 && aspectRatio <= aspect1_7)
            return Vector2.Lerp(size1_6, size1_7, (aspectRatio - aspect1_6) / (aspect1_7 - aspect1_6));
        else
            return Vector2.Lerp(size1_7, size2_3, (aspectRatio - aspect1_7) / (aspect2_3 - aspect1_7));
    }

    private Vector2 InterpolateOffset(float aspectRatio)
    {
        float aspect1_6 = 1.6f;
        float aspect1_7 = 1.7f;
        float aspect2_3 = 2.3f;

        if (aspectRatio <= aspect1_6)
            return offset1_6;
        else if (aspectRatio >= aspect2_3)
            return offset2_3;
        else if (aspectRatio > aspect1_6 && aspectRatio <= aspect1_7)
            return Vector2.Lerp(offset1_6, offset1_7, (aspectRatio - aspect1_6) / (aspect1_7 - aspect1_6));
        else
            return Vector2.Lerp(offset1_7, offset2_3, (aspectRatio - aspect1_7) / (aspect2_3 - aspect1_7));
    }
}
