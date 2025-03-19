using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    public abstract class UINavigationButtonBase : UISelectableButtonBase
    {
        [Header("Navigation Manager Reference")]
        [SerializeField] protected UINavigationButtonManager navigationManager;

        [SerializeField] private string buttonName;
        [SerializeField] private TextMeshProUGUI tmpButtonName;
        [SerializeField] private GameObject panelButtonName;

        [Header("Pop Up Setting")]
        [SerializeField] private float offsetX;
        [SerializeField] private float offsetY;

        protected override void Awake()
        {
            base.Awake();

            if (navigationManager == null)
            {
                Debug.LogError($"[UINavigationButtonBase] No UINavigationButtonManager assigned for {gameObject.name}");
            }
            panelButtonName.SetActive(true);
            tmpButtonName.text = buttonName;
            panelButtonName.SetActive(false);

            //panelPopUp.SetActive(true);
            tmpPopUp.text = buttonName;
        }

        protected override void Start()
        {
            base.Start();
            if (navigationManager != null)
            {
                navigationManager.RegisterButton(this);
            }

        }

        protected override void OnButtonClicked()
        {
            if (navigationManager != null)
            {
                navigationManager.SelectButton(this);
            }

            tmpButtonName.enabled = true;
            panelButtonName.SetActive(true);
        }

        public override void SelectButton()
        {
            if (navigationManager != null)
            {
                navigationManager.SelectButton(this);
            }

            //tmpButtonName.enabled = true;
            panelButtonName.SetActive(true);
            base.SelectButton();
            HideTooltip(); ;
        }

        public override void DeselectButton()
        {
            base.DeselectButton();
            //tmpButtonName.enabled = false;
            panelButtonName.SetActive(false);
            HideTooltip(); ;
        }

        #region Event
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            ShowTooltip();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
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

            if (!navigationManager.CheckActiveButton(this))
            {
                currentPopup = PopUpUIManager.Instance.ShowPopup(
                                PopupType.Tooltip,
                                popupTargetTransform,
                                $"{buttonName}",
                                offsetX, offsetY
                            );
            }
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
