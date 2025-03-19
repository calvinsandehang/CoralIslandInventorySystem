using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class UIHelper
{
    /// <summary>
    /// Toggles a CanvasGroup's visibility and interactivity with an optional transition effect.
    /// </summary>
    /// <param name="cg">The CanvasGroup to toggle.</param>
    /// <param name="isActive">True to show and enable interaction, false to hide and disable interaction.</param>
    /// <param name="useTransition">Whether to gradually change the alpha value.</param>
    /// <param name="duration">The duration of the transition.</param>
    public static void ToggleCanvasGroup(CanvasGroup cg, bool isActive, bool useTransition = false, float duration = 0.3f)
    {
        if (cg == null) return;

        float targetAlpha = isActive ? 1f : 0f;

        if (useTransition)
        {
            cg.DOFade(targetAlpha, duration).OnComplete(() =>
            {
                cg.interactable = isActive;
                cg.blocksRaycasts = isActive;
            });
        }
        else
        {
            cg.alpha = targetAlpha;
            cg.interactable = isActive;
            cg.blocksRaycasts = isActive;
        }
    }

    /// <summary>
    /// Sets the alpha value of an Image component with an optional transition effect.
    /// </summary>
    /// <param name="image">The Image component to modify.</param>
    /// <param name="alpha">The desired alpha value (0f = fully transparent, 1f = fully opaque).</param>
    /// <param name="useTransition">Whether to gradually change the alpha value.</param>
    /// <param name="duration">The duration of the transition.</param>
    public static void SetImageAlpha(Image image, float alpha, bool useTransition = false, float duration = 0.3f)
    {
        if (image == null) return;

        alpha = Mathf.Clamp01(alpha);

        if (useTransition)
        {
            image.DOFade(alpha, duration);
        }
        else
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    /// <summary>
    /// Swaps the sprite of an Image component with an optional transition effect.
    /// </summary>
    /// <param name="image">The Image component to modify.</param>
    /// <param name="newSprite">The new sprite to set.</param>
    /// <param name="useTransition">Whether to fade out and in during the swap.</param>
    /// <param name="duration">The duration of the transition.</param>
    public static void SwapImageSprite(Image image, Sprite newSprite, bool useTransition = false, float duration = 0.3f)
    {
        if (image == null || newSprite == null) return;

        if (useTransition)
        {
            // Fade out, swap sprite, then fade in
            image.DOFade(0f, duration / 2).OnComplete(() =>
            {
                image.sprite = newSprite;
                image.DOFade(1f, duration / 2);
            });
        }
        else
        {
            // Instantly swap the sprite
            image.sprite = newSprite;
        }
    }

    /// <summary>
    /// Shakes a RectTransform with configurable strength and resets its position.
    /// </summary>
    public static void Shake(RectTransform rectTransform, float duration, float strength, int vibrato, float randomness)
    {
        if (rectTransform == null) return;

        Vector3 originalPosition = rectTransform.anchoredPosition;
        rectTransform.DOKill(); // Stop any existing shake animation

        rectTransform.DOShakeAnchorPos(duration, strength, vibrato, randomness)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => rectTransform.anchoredPosition = originalPosition);
    }

    /// <summary>
    /// Flashes an Image component between a given color and its original color.
    /// </summary>
    public static void FlashColor(Image image, Color flashColor, float duration, float frequency)
    {
        if (image == null) return;

        Color originalColor = image.color;
        image.DOKill(); // Stop existing flash effect

        Sequence flashSequence = DOTween.Sequence();
        flashSequence.Append(image.DOColor(flashColor, frequency))
                     .Append(image.DOColor(originalColor, frequency))
                     .SetLoops((int)(duration / (frequency * 2)), LoopType.Yoyo);
    }

}
