using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    public class UINavigationButtonManager : MonoBehaviour
    {
        [SerializeField] private List<UISelectableButtonBase> navigationButtons = new List<UISelectableButtonBase>();
        [SerializeField] private List<NavigationTypeCanvasGroupPair> navigationCanvasGroups = new List<NavigationTypeCanvasGroupPair>();

        private UISelectableButtonBase selectedButton;
        [SerializeField] private int initialIndex = 0;
        private int currentIndex = 0;
        [SerializeField] private float fadeDuration = 0.5f; // Duration for smooth transitions

        private void Start()
        {
            currentIndex = initialIndex;
            if (navigationButtons.Count > 0)
            {
                SelectButton(navigationButtons[currentIndex]);
            }
        }

        public void RegisterButton(UISelectableButtonBase button)
        {
            if (!navigationButtons.Contains(button))
            {
                navigationButtons.Add(button);
            }
        }

        public void SelectButton(UISelectableButtonBase button)
        {
            if (selectedButton == button) return;

            if (selectedButton != null)
            {
                selectedButton.DeselectButton();
            }

            selectedButton = button;
            selectedButton.SelectButton();

            // Try to find and activate the corresponding canvas groups
            if (selectedButton.TryGetComponent(out UINavigationButtonBase navButton))
            {
                ActivateCanvasGroups(navButton.NavigationType);
            }
        }

        private void ActivateCanvasGroups(NavigationType type)
        {
            foreach (var pair in navigationCanvasGroups)
            {
                bool isTarget = pair.NavigationType == type;

                foreach (var canvasGroup in pair.CanvasGroups)
                {
                    if (canvasGroup == null) continue;

                    // Use UIHelper to gradually fade in the selected CanvasGroups and fade out the others
                    UIHelper.ToggleCanvasGroup(canvasGroup, isTarget, useTransition: true, duration: fadeDuration);
                }
            }
        }

        public void NavigateLeft()
        {
            if (navigationButtons.Count == 0) return;

            currentIndex = navigationButtons.IndexOf(selectedButton);
            int newIndex = (currentIndex - 1 + navigationButtons.Count) % navigationButtons.Count;
            SelectButton(navigationButtons[newIndex]);
        }

        public void NavigateRight()
        {
            if (navigationButtons.Count == 0) return;

            currentIndex = navigationButtons.IndexOf(selectedButton);
            int newIndex = (currentIndex + 1) % navigationButtons.Count;
            SelectButton(navigationButtons[newIndex]);
        }

        internal bool CheckActiveButton(UINavigationButtonBase uINavigationButtonBase)
        {
            return uINavigationButtonBase == selectedButton;
        }
    }
}
