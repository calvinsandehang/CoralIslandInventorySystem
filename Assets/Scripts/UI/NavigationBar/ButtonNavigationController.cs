using UnityEngine;
using UnityEngine.UI;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    public class ButtonNavigationController : MonoBehaviour
    {
        [SerializeField] private Button buttonLeft; // Q Button
        [SerializeField] private Button buttonRight; // E Button

        private void Start()
        {
            buttonLeft.onClick.AddListener(NavigateLeft);
            buttonRight.onClick.AddListener(NavigateRight);
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
            ButtonNavigationBarManager.Instance.NavigateLeft();
        }

        private void NavigateRight()
        {
            ButtonNavigationBarManager.Instance.NavigateRight();
        }
    }
}
