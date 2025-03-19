using TMPro;
using UnityEngine;

namespace StairwayGames.CoralIsland.UI.ButtonSystem
{
    public abstract class UINavigationButtonBase : UISelectableButtonBase
    {
        [Header("Navigation Manager Reference")]
        [SerializeField] protected UINavigationButtonManager navigationManager;

        [SerializeField] private string buttonName;
        [SerializeField] private TextMeshProUGUI tmpButtonName;
        [SerializeField] private GameObject panelButtonName;

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

            panelPopUp.SetActive(true);
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
        }

        public override void DeselectButton()
        {
            base.DeselectButton();
            //tmpButtonName.enabled = false;
            panelButtonName.SetActive(false);
        }
    }
}
