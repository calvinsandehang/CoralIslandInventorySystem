using System;
using Sirenix.OdinInspector;
using StairwayGames.CoralIsland.UI.ButtonSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StairwayGames.CoralIsland.UI.Recipe
{
    public class UIContainerRecipe : UISelectableButtonBase
    {
        [Header("Color Status")]
        [SerializeField] protected Color craftable = Color.white;
        [SerializeField] protected Color uncraftable = Color.white;
        [SerializeField] protected Color notDiscovered = Color.white;

        [Header("State Images")]
        [SerializeField] private Image undiscoveredImage;
        [SerializeField] private Image discoveredImage;

        [Header("Crafting & Pin System")]
        [SerializeField] private GameObject craftingIconObject; // Changed to GameObject reference
        [SerializeField] private GameObject pinIconObject; // Changed to GameObject reference

        private CraftingRecipeSO recipeData;

        public void Initialize(CraftingRecipeSO recipe)
        {
            recipeData = recipe;

            // Set discovered or undiscovered state
            bool isDiscovered = recipe.IsDiscovered;
            undiscoveredImage.gameObject.SetActive(!isDiscovered);
            discoveredImage.gameObject.SetActive(isDiscovered);

            // Set item sprite
            discoveredImage.sprite = recipe.ItemSprite;
            undiscoveredImage.sprite = recipe.UndiscoveredSprite;

            // Set craftable state
            craftingIconObject.SetActive(recipe.IsCraftable);

            // Default pin state
            pinIconObject.SetActive(false);

            SetColorStatus();
        }

        private void SetColorStatus()
        {
            if (recipeData.IsCraftable)
                buttonVisual.color = craftable;
            else if (!recipeData.IsCraftable && recipeData.IsDiscovered)
            {
                buttonVisual.color = uncraftable;
            }
            else
            {
                buttonVisual.color = notDiscovered;
            }
        }

        /// <summary>
        /// Toggles the pin state.
        /// </summary>
        public void TogglePin()
        {
            bool isPinned = pinIconObject.activeSelf;
            pinIconObject.SetActive(!isPinned);
        }

        #region Vfx
        public override void ToggleHoveredVfx()
        {
            if (!recipeData.IsDiscovered) return;

            base.ToggleHoveredVfx();
        }
        #endregion
        #region Event
        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        protected override void OnButtonClicked()
        {
            base.OnButtonClicked();
            //TogglePin();
            CoralIslandEvent.OnRecipeChosen(recipeData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (recipeData.IsDiscovered)
                ShowTooltip();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (recipeData.IsDiscovered)
                HideTooltip();
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
                $"{recipeData.ItemName}",
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
