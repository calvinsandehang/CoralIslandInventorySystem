using UnityEngine;
using UnityEngine.UI;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    public class ButtonNavigationController : MonoBehaviour
    {
        [SerializeField] private Button buttonLeft; // Q Button
        [SerializeField] private Button buttonRight; // E Button
        [SerializeField] private UINavigationButtonManager navigationManager;

        private void Start()
        {
            if (buttonLeft != null) buttonLeft.onClick.AddListener(NavigateLeft);
            if (buttonRight != null) buttonRight.onClick.AddListener(NavigateRight);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                NavigateLeft();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                NavigateRight();
            }
        }

        private void NavigateLeft()
        {
            if (navigationManager != null)
            {
                navigationManager.NavigateLeft();
            }
            else
            {
                Debug.LogError("[ButtonNavigationController] Navigation Manager is not assigned!");
            }
        }

        private void NavigateRight()
        {
            if (navigationManager != null)
            {
                navigationManager.NavigateRight();
            }
            else
            {
                Debug.LogError("[ButtonNavigationController] Navigation Manager is not assigned!");
            }
        }
    }
}
