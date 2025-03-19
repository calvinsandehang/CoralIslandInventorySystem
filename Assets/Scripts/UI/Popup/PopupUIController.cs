using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private RectTransform popupRect;
    [SerializeField] private Image popupImage; // For flashing effect
    [SerializeField] private CanvasGroup canvasGroup; // For fade-out effect

    [Header("Animation Settings")]
    [SerializeField] private bool enableShake;
    [SerializeField] private bool enableFlashing;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 1f;
    [SerializeField] private float flashFrequency = 0.2f;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 20f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;

    [Header("Fade Out Settings")]
    [SerializeField] private bool enableFadeOut = true; // Toggle fade-out effect
    [SerializeField] private float fadeOutDelay = 1.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// Initializes the popup with a text message and starts optional animations.
    /// </summary>
    /// <param name="message">Text to display</param>
    public void Initialize(string message)
    {
        // Stop any ongoing animations to prevent conflicts
        popupRect?.DOKill();
        popupImage?.DOKill();
        canvasGroup?.DOKill();

        if (popupText != null)
        {
            popupText.text = message;
        }
        else
        {
            Debug.LogWarning("[PopupUIController] TextMeshProUGUI is missing!");
        }

        // Reset transparency
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        // Apply Shake
        if (enableShake && popupRect != null)
        {
            UIHelper.Shake(popupRect, shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
        }

        // Apply Flashing
        if (enableFlashing && popupImage != null)
        {
            UIHelper.FlashColor(popupImage, flashColor, flashDuration, flashFrequency);
        }

        // Apply Fade-Out if enabled
        if (enableFadeOut)
        {
            FadeOutAndDestroy();
        }
    }

    /// <summary>
    /// Gradually fades out the popup and destroys it after.
    /// </summary>
    private void FadeOutAndDestroy()
    {
        if (canvasGroup != null)
        {
            canvasGroup.DOFade(0f, fadeOutDuration).SetDelay(fadeOutDelay).OnComplete(() =>
            {
                Destroy(gameObject); // Automatically hide tooltip
            });
        }
    }
}
