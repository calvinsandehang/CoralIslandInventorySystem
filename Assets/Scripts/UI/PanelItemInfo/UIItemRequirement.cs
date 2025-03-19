using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemRequirement : MonoBehaviour
{
    [SerializeField] private Image imgItemReq;
    [SerializeField] private TextMeshProUGUI tmpItemReqName;
    [SerializeField] private TextMeshProUGUI tmpItemReqAmount;

    [SerializeField] private Color colorReqFulfilled1, colorReqFulfilled2, colorReqNotFulfilled1, colorReqNotFulfilled2;

    public void Initialize(int amountNeeded, ItemSO itemSO)
    {
        imgItemReq.sprite = itemSO.ItemSprite;
        tmpItemReqName.text = $"{itemSO.ItemName}";

        string needed = amountNeeded.ToString();
        string owned = itemSO.Amount.ToString();

        if (itemSO.Amount >= amountNeeded)
        {
            tmpItemReqName.color = colorReqFulfilled1;

            // Apply colors using TMP Rich Text Tags
            string ownedColored = $"<color=#{ColorUtility.ToHtmlStringRGB(colorReqFulfilled2)}>{owned}</color>";
            string neededColored = $"<color=#{ColorUtility.ToHtmlStringRGB(colorReqFulfilled1)}>{needed}</color>";

            tmpItemReqAmount.text = $"{ownedColored}/{neededColored}";
        }
        else
        {
            tmpItemReqName.color = colorReqNotFulfilled1;

            // Apply colors using TMP Rich Text Tags
            string ownedColored = $"<color=#{ColorUtility.ToHtmlStringRGB(colorReqNotFulfilled2)}>{owned}</color>";
            string neededColored = $"<color=#{ColorUtility.ToHtmlStringRGB(colorReqNotFulfilled1)}>{needed}</color>";

            tmpItemReqAmount.text = $"{ownedColored}/{neededColored}";
        }
    }

}
