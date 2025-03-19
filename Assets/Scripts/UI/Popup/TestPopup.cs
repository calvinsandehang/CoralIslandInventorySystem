using Sirenix.OdinInspector;
using UnityEngine;

public class TestPopup : MonoBehaviour
{
    [SerializeField] private RectTransform popupTargetTransform; // Target UI element

    private GameObject currentPopup;

    [Button]
    public void ShowTooltip()
    {
        if (PopUpUIManager.Instance == null)
        {
            Debug.LogError("[TestPopup] PopUpUIManager instance not found.");
            return;
        }

        // Prevent multiple popups from stacking
        if (currentPopup != null)
        {
            Debug.LogWarning("[TestPopup] A popup is already active.");
            return;
        }

        currentPopup = PopUpUIManager.Instance.ShowPopup(
            PopupType.Tooltip,
            popupTargetTransform,
            "This is a tooltip!",
            20f, 30f
        );
    }

    [Button]
    public void HideTooltip()
    {
        if (PopUpUIManager.Instance == null)
        {
            Debug.LogError("[TestPopup] PopUpUIManager instance not found.");
            return;
        }

        if (currentPopup == null)
        {
            Debug.LogWarning("[TestPopup] No popup to hide.");
            return;
        }

        PopUpUIManager.Instance.HidePopup(currentPopup, PopupType.Tooltip);
        currentPopup = null;
    }
}
