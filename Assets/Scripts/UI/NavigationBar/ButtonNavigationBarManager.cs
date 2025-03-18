using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    [RequireComponent(typeof(ButtonNavigationController))]
    public class ButtonNavigationBarManager : MonoBehaviour
    {
        public static ButtonNavigationBarManager Instance { get; private set; }

        [SerializeField] private List<UIBtnNavigationBar> navigationButtons = new List<UIBtnNavigationBar>();
        [SerializeField] private int initialIndex = 3;
        private int currentIndex;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            currentIndex = initialIndex;

            if (navigationButtons.Count > 0)
            {
                //navigationButtons[currentIndex].IsSelected = true;
                SelectButton(navigationButtons[currentIndex]);
            }
        }

        public void SelectButton(UIBtnNavigationBar button)
        {
            currentIndex = navigationButtons.IndexOf(button);
            for (int i = 0; i < navigationButtons.Count; i++)
            {
                if (i == currentIndex) continue;

                navigationButtons[i].DeselectButton();
            }

            button.SelectButton();
        }

        public void NavigateLeft()
        {
            if (navigationButtons.Count == 0) return;

            currentIndex = (currentIndex - 1 + navigationButtons.Count) % navigationButtons.Count;
            SelectButton(navigationButtons[currentIndex]);
        }

        public void NavigateRight()
        {
            if (navigationButtons.Count == 0) return;

            currentIndex = (currentIndex + 1) % navigationButtons.Count;
            SelectButton(navigationButtons[currentIndex]);
        }

        public void RegisterButton(UIBtnNavigationBar button)
        {
            if (!navigationButtons.Contains(button))
            {
                navigationButtons.Add(button);
            }
        }
    }
}
