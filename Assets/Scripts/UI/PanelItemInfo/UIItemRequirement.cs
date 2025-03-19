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

    public void Initialize(int amountNeeded, ItemSO itemSO)
    {
        imgItemReq.sprite = itemSO.ItemSprite;
        tmpItemReqName.text = $"{itemSO.ItemName}";
        tmpItemReqAmount.text = $"{itemSO.Amount}/{amountNeeded}";
    }
}
