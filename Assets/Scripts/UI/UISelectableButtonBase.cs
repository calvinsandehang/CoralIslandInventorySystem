using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    [RequireComponent(typeof(Button))]
    public abstract class UISelectableButtonBase : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IButtonSfx, IButtonVfx, IButtonPopUpInfo
    {
        protected Button button;

        [Header("Color Settings")]
        [SerializeField] protected Color normalColor = Color.white;
        [SerializeField] protected Color hoverColor = Color.gray;
        [SerializeField] protected Color selectedColor = Color.green;

        [Header("Vfx")]
        [SerializeField] protected Image buttonVisual;
        [SerializeField] protected Image frame;
        [SerializeField] protected float targetAlpha = 0.4f;

        [Header("Pop Up")]
        [SerializeField] protected GameObject panelPopUp;
        [SerializeField] protected TextMeshProUGUI tmpPopUp;

        [Header("Audio Settings")]
        [SerializeField] protected string hoverSfx = "SfxUI0";
        [SerializeField] protected string clickSfx = "SfxUI1";

        public bool IsSelected { get; private set; } = false;
        public bool IsHovered { get; private set; } = false;

        public bool SelectedVfxActive => IsSelected;
        public bool HoveredVfxActive => IsHovered;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();

            if (buttonVisual == null)
            {
                buttonVisual = GetComponent<Image>();
            }

            //buttonVisual.color = normalColor;


            UIHelper.SetImageAlpha(frame, 0f);
        }

        protected virtual void Start()
        {
            if (panelPopUp != null)
            {
                panelPopUp.SetActive(false);
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            if (!IsSelected && button.interactable)
            {
                PlayHoverSfx();
                ToggleHoveredVfx();
                //ToggleButtonPopUpInfo();
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            if (!IsSelected)
            {
                ToggleHoveredVfx();
                //ToggleButtonPopUpInfo();
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (button.interactable)
            {
                OnButtonClicked();
            }
        }

        protected virtual void OnButtonClicked()
        {
            // Can be overridden for different button logic
        }

        public virtual void SelectButton()
        {
            IsHovered = false;
            ToggleHoveredVfx();
            //ToggleButtonPopUpInfo();

            PlayClickPositiveSfx();
            IsSelected = true;
            buttonVisual.color = selectedColor;
            ToggleSelectedVfx();
        }

        public virtual void DeselectButton()
        {
            IsSelected = false;
            //buttonVisual.color = normalColor;
            ToggleSelectedVfx();
        }

        #region Vfx        
        [Button]
        public virtual void ToggleSelectedVfx()
        {
            buttonVisual.color = IsSelected ? selectedColor : normalColor;
        }

        public virtual void ToggleHoveredVfx()
        {
            float alphaValue = IsHovered ? targetAlpha : 0f;
            UIHelper.SetImageAlpha(frame, alphaValue);
        }
        #endregion

        #region Audio
        public virtual void PlayHoverSfx()
        {
            AudioSystemManager.Instance.PlaySfx(AudioDatabaseType.Gameplay, hoverSfx, 0.4f);
        }

        public virtual void PlayClickPositiveSfx()
        {
            AudioSystemManager.Instance.PlaySfx(AudioDatabaseType.Gameplay, clickSfx, 0.6f);
        }

        public virtual void PlayClickNegativeSfx() { }
        #endregion

        #region PopUp
        public virtual void ToggleButtonPopUpInfo()
        {
            if (panelPopUp != null)
            {
                panelPopUp.SetActive(IsHovered);
            }
        }
        #endregion
    }
}
