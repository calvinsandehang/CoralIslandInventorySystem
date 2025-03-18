using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOButtonNavigation", menuName = "SOData/ButtonNavigation")]
public class SOButtonNavigation : ScriptableObject
{

    [SerializeField] private string buttonName;

    [Header("Color Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private Color selectedColor = Color.green;




}
