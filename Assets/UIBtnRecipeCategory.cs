using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    public class UIBtnRecipeCategory : UINavigationButtonBase
    {
        //[SerializeField] private GameObject panelButtonName;
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            //panelButtonName.SetActive(false);
        }

        #region Selection Logic
        public override void SelectButton()
        {
            base.SelectButton();
            //panelButtonName.SetActive(true);
        }

        public override void DeselectButton()
        {
            base.DeselectButton();
            //panelButtonName.SetActive(false);
        }
        #endregion

        #region Vfx        
        [Button]
        public override void ToggleSelectedVfx()
        {
            // No animation, just color change
            buttonVisual.color = IsSelected ? selectedColor : normalColor;
        }

        public override void ToggleHoveredVfx()
        {
            float alphaValue = IsHovered ? targetAlpha : 0f;
            UIHelper.SetImageAlpha(frame, alphaValue);
        }
        #endregion
    }
}
