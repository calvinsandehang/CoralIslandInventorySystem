using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PanelItemInfo : MonoBehaviour, ISubscriber
{
    [SerializeField] private RectTransform panelItemInfo;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 20f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;

    [Header("Resize Settings")]
    [SerializeField] private float resizeDuration = 0.5f;
    [SerializeField] private float resizeMultiplier = 1.5f; // Scale factor for resizing
    [SerializeField] private bool playReverse = true; // If true, it resizes back to the original size

    [Header("Animation Ease Settings")]
    [SerializeField] private Ease scaleUpEase = Ease.OutBack;
    [SerializeField] private Ease scaleDownEase = Ease.InOutBack;

    [Header("Flash Settings")]
    [SerializeField] private Image panelImage; // UI Image to flash
    [SerializeField] private float flashDuration = 1f;
    [SerializeField] private float flashFrequency = 0.2f;
    [SerializeField] private Color flashColor = Color.red;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private void Start()
    {
        Subscribe();
        originalScale = panelItemInfo.localScale; // Store the original scale
        originalPosition = panelItemInfo.anchoredPosition; // Store the original position
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    /// <summary>
    /// Shakes the panel and resets it to the original position after.
    /// </summary>
    [Button]
    public void ShakePanel()
    {
        if (panelItemInfo == null)
        {
            Debug.LogError("[PanelItemInfo] Panel RectTransform is not assigned!");
            return;
        }

        UIHelper.Shake(panelItemInfo, shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
    }

    /// <summary>
    /// Flashes the panel's image.
    /// </summary>
    [Button]
    public void FlashPanel()
    {
        if (panelImage == null)
        {
            Debug.LogError("[PanelItemInfo] Panel Image component is not assigned!");
            return;
        }

        UIHelper.FlashColor(panelImage, flashColor, flashDuration, flashFrequency);
    }

    /// <summary>
    /// Resizes the panel, then returns it to its original size if enabled.
    /// </summary>
    [Button]
    public void ResizePanel()
    {
        if (panelItemInfo == null)
        {
            Debug.LogError("[PanelItemInfo] Panel RectTransform is not assigned!");
            return;
        }

        Vector3 targetScale = originalScale * resizeMultiplier;
        Sequence resizeSequence = DOTween.Sequence();
        resizeSequence.Append(panelItemInfo.DOScale(targetScale, resizeDuration).SetEase(scaleUpEase));

        if (playReverse)
        {
            resizeSequence.Append(panelItemInfo.DOScale(originalScale, resizeDuration).SetEase(scaleDownEase));
        }
    }

    /// <summary>
    /// Instantly resets the panel size to its original scale.
    /// </summary>
    [Button]
    public void ResetPanelSize()
    {
        if (panelItemInfo == null)
        {
            Debug.LogError("[PanelItemInfo] Panel RectTransform is not assigned!");
            return;
        }

        panelItemInfo.DOScale(originalScale, resizeDuration).SetEase(scaleDownEase);
    }

    public void Subscribe()
    {
        CoralIslandEvent.OnRecipeChosen += OnRecipeChosen;
    }

    public void Unsubscribe()
    {
        CoralIslandEvent.OnRecipeChosen -= OnRecipeChosen;
    }

    private void OnRecipeChosen(CraftingRecipeSO sO)
    {
        ResizePanel();
    }
}
