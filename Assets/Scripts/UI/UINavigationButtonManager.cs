using System;
using System.Collections.Generic;
using UnityEngine;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    public class UINavigationButtonManager : MonoBehaviour
    {
        [SerializeField] private List<UISelectableButtonBase> navigationButtons = new List<UISelectableButtonBase>();
        private UISelectableButtonBase selectedButton;
        [SerializeField] private int initialIndex = 0;
        int currentIndex = 0;
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
    }
}
