using System;

public static class UIInventoryEvent
{
    public static event Action<float> OnAspectRatioUpdated;

    public static void BroadcastAspectRatioChange(float aspectRatio)
    {
        OnAspectRatioUpdated?.Invoke(aspectRatio);
    }
}
