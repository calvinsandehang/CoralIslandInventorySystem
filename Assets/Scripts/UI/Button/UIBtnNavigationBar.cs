using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    [RequireComponent(typeof(Button))]
    public class UIBtnNavigationBar : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler, IPointerClickHandler, IButtonSfx, IButtonVfx, IButtonPopUpInfo
    {
        private Button button;
        private Vector3 originalPosition;

        [Header("Color Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color hoverColor = Color.gray;
        [SerializeField] private Color selectedColor = Color.green;

        [Header("Text Settings")]
        [SerializeField] private string buttonName;
        [SerializeField] private TextMeshProUGUI tmpButtonName;

        [Header("Animation Settings")]
        [SerializeField] private float moveUpDistance = 20f;
        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private Ease moveUpEaseType = Ease.OutBack; // Recommended: Overshoot effect
        [SerializeField] private Ease moveDownEaseType = Ease.OutQuad; // Smooth movement down

        [Header("Vfx")]
        [SerializeField] private Image frame; // Frame for visual effect
        [SerializeField] private float targetAlpha = 0.4f;
        [SerializeField] private Image buttonVisual;

        [Header("Pop Up")]
        [SerializeField] private GameObject panelPopUp;
        [SerializeField] private TextMeshProUGUI tmpPopUp;


        public bool IsSelected = false;
        public bool IsHovered = false;

        public bool SelectedVfxActive => IsSelected;
        public bool HoveredVfxActive => IsHovered;

        public bool PopUpInfoActive => throw new System.NotImplementedException();

        private void Awake()
        {
            button = GetComponent<Button>();

            if (buttonVisual == null)
            {
                buttonVisual = GetComponent<Image>();
            }

            buttonVisual.color = normalColor;
            tmpButtonName.text = buttonName;
            tmpButtonName.enabled = false;

            panelPopUp.SetActive(true);
            tmpPopUp.text = buttonName;
            panelPopUp.SetActive(false);

            originalPosition = buttonVisual.rectTransform.localPosition;

            // Initialize frame alpha to 0            
            UIHelper.SetImageAlpha(frame, 0f);
        }

        private void Start()
        {
            ButtonNavigationBarManager.Instance.RegisterButton(this);

            UIHelper.SetImageAlpha(buttonVisual, 0.1f);
        }
        #region Unity Event
        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            if (!IsSelected && button.interactable)
            {
                PlayHoverSfx();
                ToggleHoveredVfx();
                ToggleButtonPopUpInfo();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            if (!IsSelected)
            {
                buttonVisual.color = normalColor;
                ToggleHoveredVfx();
                ToggleButtonPopUpInfo();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (button.interactable)
            {
                ButtonNavigationBarManager.Instance.SelectButton(this);
            }
        }
        #endregion
        public void SelectButton()
        {
            IsHovered = false;
            ToggleHoveredVfx();
            ToggleButtonPopUpInfo();

            PlayClickPositiveSfx();
            IsSelected = true;
            buttonVisual.color = selectedColor;
            tmpButtonName.enabled = true;
            ToggleSelectedVfx();
        }

        public void DeselectButton()
        {
            IsSelected = false;
            buttonVisual.color = normalColor;
            tmpButtonName.enabled = false;
            ToggleSelectedVfx();

            UIHelper.SetImageAlpha(buttonVisual, 0.1f);
        }

        #region Vfx        
        [Button]
        public void ToggleSelectedVfx()
        {
            if (IsSelected)
            {
                // Move button up with overshoot effect
                buttonVisual.rectTransform.DOLocalMoveY(originalPosition.y + moveUpDistance, animationDuration)
                    .SetEase(moveUpEaseType);
            }
            else
            {
                // Move button back down smoothly
                buttonVisual.rectTransform.DOLocalMoveY(originalPosition.y, animationDuration)
                    .SetEase(moveDownEaseType);
            }
        }

        public void ToggleHoveredVfx()
        {
            float targetAlpha = IsHovered ? this.targetAlpha : 0f;
            UIHelper.SetImageAlpha(frame, targetAlpha);
        }
        #endregion

        #region Audio
        public void PlayHoverSfx()
        {
            AudioSystemManager.Instance.PlaySfx(AudioDatabaseType.Gameplay, "SfxUI0", 0.4f);
        }

        public void PlayClickPositiveSfx()
        {
            AudioSystemManager.Instance.PlaySfx(AudioDatabaseType.Gameplay, "SfxUI1", 0.6f);
        }

        public void PlayClickNegativeSfx()
        {
            // Optional negative feedback sound
        }
        #endregion

        #region PopUp


        public void ToggleButtonPopUpInfo()
        {
            panelPopUp.SetActive(IsHovered);
        }
        #endregion
    }
}
