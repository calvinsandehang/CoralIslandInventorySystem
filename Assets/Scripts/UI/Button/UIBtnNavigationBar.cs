using UnityEngine;
using TMPro;
using DG.Tweening;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    public class UIBtnNavigationBar : UINavigationButtonBase
    {
        [Header("Animation Settings")]
        [SerializeField] private float moveUpDistance = 20f;
        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private Ease moveUpEaseType = Ease.OutBack;
        [SerializeField] private Ease moveDownEaseType = Ease.OutQuad;

        private Vector3 originalPosition;

        protected override void Awake()
        {
            base.Awake();
            originalPosition = buttonVisual.rectTransform.localPosition;
        }

        public override void SelectButton()
        {
            base.SelectButton();
            AnimateButton(true);
        }

        public override void DeselectButton()
        {
            base.DeselectButton();
            AnimateButton(false);
        }

        private void AnimateButton(bool isSelected)
        {
            buttonVisual.rectTransform.DOLocalMoveY(
                isSelected ? originalPosition.y + moveUpDistance : originalPosition.y,
                animationDuration
            ).SetEase(isSelected ? moveUpEaseType : moveDownEaseType);
        }
    }
}
