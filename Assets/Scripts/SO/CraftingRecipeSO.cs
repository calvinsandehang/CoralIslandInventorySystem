using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Game/Crafting Recipe")]
public class CraftingRecipeSO : ItemSO
{
    [Header("Recipe Info")]
    [SerializeField] private RecipeCategory category;

    [Header("Crafting Requirements")]
    [SerializeField] private List<ItemRequirement> itemRequirements = new List<ItemRequirement>();

    [Header("Visuals")]
    [SerializeField] private Sprite undiscoveredSprite;

    [Header("Simulation State")]
    [SerializeField] private bool isCraftable = false;
    [SerializeField] private bool isDiscovered = false;

    // Public Properties
    public RecipeCategory Category => category;
    public List<ItemRequirement> ItemRequirements => itemRequirements;
    public Sprite UndiscoveredSprite => undiscoveredSprite;
    public bool IsCraftable => isCraftable;
    public bool IsDiscovered => isDiscovered;
}

[Serializable]
public class ItemRequirement
{
    public ItemSO Item;
    public int AmountNeeded;
}
