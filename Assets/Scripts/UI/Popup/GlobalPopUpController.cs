using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GlobalPopUpController : MonoBehaviour, ISubscriber
{
    #region Popup
    [SerializeField] private Transform popupTargetTransform; // Target UI element
    [SerializeField] private float offsetX, offsetY;

    private GameObject currentPopup;

    private void Start()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    [Button]
    public void ShowTooltipInsufficientMaterial()
    {
        if (PopUpUIManager.Instance == null)
        {
            Debug.LogError("[GlobalPopUpController] PopUpUIManager instance not found.");
            return;
        }

        // Hide the current popup before showing a new one
        HideTooltip();

        // Show a new popup
        currentPopup = PopUpUIManager.Instance.ShowPopup(
            PopupType.Notification,
            popupTargetTransform,
            $"You have insufficient\nmaterial for this recipe",
            offsetX, offsetY
        );

        // Initialize animations in PopupUIController
        PopupUIController popupController = currentPopup.GetComponent<PopupUIController>();
        if (popupController != null)
        {
            popupController.Initialize($"You have insufficient\nmaterial for this recipe");
        }
    }

    [Button]
    public void HideTooltip()
    {
        if (PopUpUIManager.Instance == null)
        {
            Debug.LogError("[GlobalPopUpController] PopUpUIManager instance not found.");
            return;
        }

        if (currentPopup == null)
        {
            return; // No popup to hide
        }

        PopUpUIManager.Instance.HidePopup(currentPopup, PopupType.Notification);
        currentPopup = null; // Reset reference after hiding
    }

    public void Subscribe()
    {
        CoralIslandEvent.OnFailCraft += ShowTooltipInsufficientMaterial;
    }

    public void Unsubscribe()
    {
        CoralIslandEvent.OnFailCraft -= ShowTooltipInsufficientMaterial;
    }
    #endregion
}
