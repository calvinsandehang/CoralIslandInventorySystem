using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoralIslandEvent
{
    public static Action<bool> OnToggleSfxSound;
    public static Action<float> OnOnSfxVolumeChanged;
    public static Action<AudioClip> OnStopSfx;

    public static Action<RecipeCategory> OnChangeRecipeCategory;
    public static Action<CraftingRecipeSO> OnRecipeChosen;
    public static Action<CraftingRecipeSO> OnTryCraft;
    public static Action OnFailCraft;
}

