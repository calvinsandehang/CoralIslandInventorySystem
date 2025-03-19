using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StairwayGames.CoralIsland.UI.ButtonSystem;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace StairwayGames.CoralIsland.UI.Inventory
{
    public class UIContainerBackpack : UISelectableButtonBase
    {
        [Header("Item Data")]
        [SerializeField] private ItemSO itemData;

        [Header("UI References")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemAmountText;

        [Header("Lock/Unlock System")]
        [SerializeField] private GameObject panelLockSlot;
        [SerializeField] private GameObject panelUnlockSlot;
        [SerializeField] private GameObject panelAmount;
        [SerializeField] private Button backpackButton;

        [Header("State Flags")]
        [SerializeField] private bool isLocked = false;

        [SerializeField] private int itemAmount = 0; // Default amount

        [Header("Color Status")]
        [SerializeField] protected Color colorHasItem = Color.white;
        [SerializeField] protected Color colorEmpty = Color.white;
        [SerializeField] protected Color colorHovered = Color.white;


        bool hasItem;

        protected override void Awake()
        {
            base.Awake();
            Initialize(itemData, itemAmount, isLocked);
        }

        #region Initialization
        /// <summary>
        /// Initializes the backpack slot with the given item and amount.
        /// </summary>
        public void Initialize(ItemSO item, int amount, bool locked)
        {
            isLocked = locked;
            itemData = item;
            itemAmount = itemData != null ? Mathf.Max(amount, 1) : 0; // Ensure at least 1 if item exists
            hasItem = itemData != null;

            UpdateSlotState();
            UpdateItemUI();
        }
        #endregion


        #region Backpack method
        /// <summary>
        /// Updates the UI state based on whether the slot is locked or unlocked.
        /// </summary>
        private void UpdateSlotState()
        {
            if (panelLockSlot != null) panelLockSlot.SetActive(isLocked);
            if (panelUnlockSlot != null) panelUnlockSlot.SetActive(!isLocked);
            if (panelAmount != null) panelAmount.SetActive(!isLocked);

            if (backpackButton != null)
            {
                backpackButton.interactable = !isLocked;
            }
        }

        /// <summary>
        /// Updates the item UI (icon and amount text).
        /// </summary>
        private void UpdateItemUI()
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = hasItem ? itemData.ItemSprite : null;
                itemIcon.gameObject.SetActive(hasItem);

                buttonVisual.color = hasItem ? colorHasItem : colorEmpty;
            }

            UpdateAmountText();
        }

        /// <summary>
        /// Updates the item amount text and handles panel visibility.
        /// </summary>
        private void UpdateAmountText()
        {
            if (itemAmountText != null)
            {
                itemAmountText.text = itemAmount > 1 ? itemAmount.ToString() : "";
            }

            if (panelAmount != null)
            {
                panelAmount.SetActive(itemAmount >= 2); // Show only if amount is at least 2
            }
        }

        /// <summary>
        /// Increases the amount of the item.
        /// </summary>
        public void IncreaseAmount(int amount)
        {
            if (isLocked || itemData == null) return;

            itemAmount += amount;
            UpdateAmountText();
        }

        /// <summary>
        /// Decreases the amount of the item. If the amount reaches zero, the slot is cleared.
        /// </summary>
        public void DecreaseAmount(int amount)
        {
            if (isLocked || itemData == null) return;

            itemAmount -= amount;
            if (itemAmount <= 0)
            {
                ClearSlot();
            }
            else
            {
                UpdateAmountText();
            }
        }

        /// <summary>
        /// Clears the backpack slot.
        /// </summary>
        public void ClearSlot()
        {
            if (isLocked) return;

            itemData = null;
            itemAmount = 0;
            UpdateItemUI();
        }

        /// <summary>
        /// Unlocks the backpack slot.
        /// </summary>
        public void UnlockSlot()
        {
            isLocked = false;
            UpdateSlotState();
        }

        /// <summary>
        /// Locks the backpack slot.
        /// </summary>
        public void LockSlot()
        {
            isLocked = true;
            UpdateSlotState();
        }
        #endregion




        #region Event
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!hasItem) return;
            buttonVisual.color = colorHovered;
            ShowTooltip();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            buttonVisual.color = hasItem ? colorHasItem : colorEmpty;
            HideTooltip();
        }

        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        protected override void OnButtonClicked()
        {
            if (isLocked || itemData == null) return; // Prevent interactions if locked or empty

            base.OnButtonClicked();
            Debug.Log($"Clicked on {itemData?.ItemName ?? "Empty Slot"}");
        }
        #endregion

        #region Popup
        [SerializeField] private Transform popupTargetTransform; // Target UI element

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
                $"{itemData.ItemName}",
                0f, 70f
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
        #endregion

    }
}
