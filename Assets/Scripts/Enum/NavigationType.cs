using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NavigationType
{
    Town,
    Journal,
    Quest,
    Crafting,
    Inventory,
    Map,
    Relationship,
    Mastery
}

[Serializable]
public class NavigationTypeCanvasGroupPair
{
    public NavigationType NavigationType;
    public List<CanvasGroup> CanvasGroups = new List<CanvasGroup>(); // Changed to a List
}
