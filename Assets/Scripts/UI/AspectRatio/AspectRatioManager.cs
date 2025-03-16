using UnityEngine;

[ExecuteAlways]
public class AspectRatioManager : MonoBehaviour
{
    private float lastAspectRatio = -1f;

    private void Start()
    {
        DetectAspectRatio();
    }

    private void Update()
    {
        DetectAspectRatio();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        DetectAspectRatio();
    }
#endif

    private void DetectAspectRatio()
    {
        float currentAspect = (float)Screen.width / Screen.height;

        if (Mathf.Abs(currentAspect - lastAspectRatio) > 0.01f)
        {
            lastAspectRatio = currentAspect;
            UIInventoryEvent.BroadcastAspectRatioChange(currentAspect); // ✅ Now broadcasting via UIInventoryEvent
            Debug.Log($"📢 [AspectRatioManager] Aspect Ratio Changed: {currentAspect}");
        }
    }
}
