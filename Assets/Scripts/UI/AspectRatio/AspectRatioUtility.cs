using UnityEngine;

public static class AspectRatioUtility
{
    private static readonly float[] predefinedAspectRatios = { 16f / 10f, 16f / 9f, 21f / 9f, 1920f / 1200f };

    public static int FindClosestAspectRatio(float aspect, int screenWidth, int screenHeight)
    {
        // Explicit check for 1920x1200 resolution
        if (screenWidth == 1920 && screenHeight == 1200)
        {
            return 3;
        }

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

    public static bool IsPredefinedAspectRatio(float aspect, int screenWidth, int screenHeight)
    {
        if (screenWidth == 1920 && screenHeight == 1200) return true;

        foreach (float predefined in predefinedAspectRatios)
        {
            if (Mathf.Abs(predefined - aspect) < 0.01f) return true;
        }
        return false;
    }

    public static Vector2 InterpolateVector2(float aspect, Vector2 value_16_10, Vector2 value_16_9, Vector2 value_21_9, Vector2 value_1920_1200)
    {
        return new Vector2(
            InterpolateValue(aspect, value_16_10.x, value_16_9.x, value_21_9.x, value_1920_1200.x),
            InterpolateValue(aspect, value_16_10.y, value_16_9.y, value_21_9.y, value_1920_1200.y)
        );
    }

    public static float InterpolateValue(float aspect, float value_16_10, float value_16_9, float value_21_9, float value_1920_1200)
    {
        if (Mathf.Approximately(aspect, predefinedAspectRatios[3])) return value_1920_1200;

        if (aspect <= predefinedAspectRatios[0])
        {
            return Mathf.Lerp(value_1920_1200, value_16_10, Mathf.InverseLerp(1920f / 1200f, predefinedAspectRatios[0], aspect));
        }
        else if (aspect <= predefinedAspectRatios[1])
        {
            return Mathf.Lerp(value_16_10, value_16_9, Mathf.InverseLerp(predefinedAspectRatios[0], predefinedAspectRatios[1], aspect));
        }
        else if (aspect <= predefinedAspectRatios[2])
        {
            return Mathf.Lerp(value_16_9, value_21_9, Mathf.InverseLerp(predefinedAspectRatios[1], predefinedAspectRatios[2], aspect));
        }
        else
        {
            return value_21_9;
        }
    }
}
