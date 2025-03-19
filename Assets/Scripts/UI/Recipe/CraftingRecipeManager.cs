using System.Collections.Generic;
using Sirenix.OdinInspector;
using StairwayGames.CoralIsland.UI.Recipe;
using UnityEngine;
using UnityEngine.UI;

namespace StairwayGames.CoralIsland.Crafting
{
    public class CraftingRecipeManager : MonoBehaviour, ISubscriber
    {
        [Header("Testing")]
        [SerializeField] private List<GameObject> testingObjects = new();

        [Header("Recipe Lists")]
        [SerializeField] private List<CraftingRecipeSO> allRecipes = new List<CraftingRecipeSO>();

        [Header("Category Lists")]
        private Dictionary<RecipeCategory, List<CraftingRecipeSO>> categorizedRecipes = new Dictionary<RecipeCategory, List<CraftingRecipeSO>>();

        [Header("Dummy Items")]
        [SerializeField] private CraftingRecipeSO dummyFarmingRecipe;
        [SerializeField] private CraftingRecipeSO dummyArtisanRecipe;
        [SerializeField] private CraftingRecipeSO dummyDecorRecipe;
        [SerializeField] private CraftingRecipeSO dummyScarecrowRecipe;
        [SerializeField] private CraftingRecipeSO dummyMiscRecipe;

        private Dictionary<RecipeCategory, CraftingRecipeSO> dummyRecipes;

        [Header("Category Limits")]
        [SerializeField] private int farmingLimit = 10;
        [SerializeField] private int artisanLimit = 10;
        [SerializeField] private int decorLimit = 10;
        [SerializeField] private int scarecrowLimit = 10;
        [SerializeField] private int miscLimit = 10;

        private Dictionary<RecipeCategory, int> categoryLimits;

        [Header("UI References")]
        [SerializeField] private Transform farmingPanel;
        [SerializeField] private Transform artisanPanel;
        [SerializeField] private Transform decorPanel;
        [SerializeField] private Transform scarecrowPanel;
        [SerializeField] private Transform miscPanel;
        [SerializeField] private Transform allRecipesPanel;

        [SerializeField] private GameObject recipePrefab;
        [SerializeField] private ScrollRect scrollRect;

        private Dictionary<RecipeCategory, Transform> categoryPanels;
        private RecipeCategory activeCategory = RecipeCategory.All;

        private void Awake()
        {
            for (int i = 0; i < testingObjects.Count; i++)
            {
                Destroy(testingObjects[i]);
            }
        }

        private void Start()
        {
            InitializeDictionaries();
            CategorizeRecipes();
            PopulateRecipePanels();
            SetActiveCategory(activeCategory);
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void InitializeDictionaries()
        {
            categorizedRecipes = new Dictionary<RecipeCategory, List<CraftingRecipeSO>>()
            {
                { RecipeCategory.Farming, new List<CraftingRecipeSO>() },
                { RecipeCategory.Artisan, new List<CraftingRecipeSO>() },
                { RecipeCategory.Decor, new List<CraftingRecipeSO>() },
                { RecipeCategory.Scarecrow, new List<CraftingRecipeSO>() },
                { RecipeCategory.Misc, new List<CraftingRecipeSO>() }
            };

            categoryLimits = new Dictionary<RecipeCategory, int>()
            {
                { RecipeCategory.Farming, farmingLimit },
                { RecipeCategory.Artisan, artisanLimit },
                { RecipeCategory.Decor, decorLimit },
                { RecipeCategory.Scarecrow, scarecrowLimit },
                { RecipeCategory.Misc, miscLimit }
            };

            dummyRecipes = new Dictionary<RecipeCategory, CraftingRecipeSO>()
            {
                { RecipeCategory.Farming, dummyFarmingRecipe },
                { RecipeCategory.Artisan, dummyArtisanRecipe },
                { RecipeCategory.Decor, dummyDecorRecipe },
                { RecipeCategory.Scarecrow, dummyScarecrowRecipe },
                { RecipeCategory.Misc, dummyMiscRecipe }
            };

            categoryPanels = new Dictionary<RecipeCategory, Transform>()
            {
                { RecipeCategory.Farming, farmingPanel },
                { RecipeCategory.Artisan, artisanPanel },
                { RecipeCategory.Decor, decorPanel },
                { RecipeCategory.Scarecrow, scarecrowPanel },
                { RecipeCategory.Misc, miscPanel },
                { RecipeCategory.All, allRecipesPanel }
            };
        }

        private void CategorizeRecipes()
        {
            foreach (var category in categorizedRecipes.Keys)
            {
                categorizedRecipes[category].Clear();
            }

            foreach (var recipe in allRecipes)
            {
                categorizedRecipes[recipe.Category].Add(recipe);
            }
        }

        private void PopulateRecipePanels()
        {
            foreach (var category in categorizedRecipes.Keys)
            {
                PopulatePanel(
                    categorizedRecipes[category],
                    categoryPanels[category],
                    dummyRecipes[category],
                    categoryLimits[category]
                );
            }

            PopulateAllRecipesPanel();
        }

        private void PopulatePanel(List<CraftingRecipeSO> recipes, Transform panel, CraftingRecipeSO dummyRecipe, int limit)
        {
            if (panel == null) return;

            int count = 0;
            foreach (var recipe in recipes)
            {
                if (count >= limit) break;
                SpawnRecipe(recipe, panel);
                count++;
            }

            while (count < limit)
            {
                SpawnRecipe(dummyRecipe, panel);
                count++;
            }
        }

        /// <summary>
        /// Populates the "All" category by first adding all real recipes and then filling the rest with dummy recipes.
        /// </summary>
        private void PopulateAllRecipesPanel()
        {
            if (allRecipesPanel == null) return;

            // Spawn all actual recipes first
            foreach (var recipe in allRecipes)
            {
                SpawnRecipe(recipe, allRecipesPanel);
            }

            // Spawn all dummy recipes afterward
            foreach (var category in dummyRecipes.Keys)
            {
                for (int i = 0; i < categoryLimits[category]; i++)
                {
                    SpawnRecipe(dummyRecipes[category], allRecipesPanel);
                }
            }
        }

        private void SpawnRecipe(CraftingRecipeSO recipe, Transform panel)
        {
            GameObject recipeUI = Instantiate(recipePrefab, panel);
            UIContainerRecipe uiContainer = recipeUI.GetComponent<UIContainerRecipe>();
            if (uiContainer != null)
            {
                uiContainer.Initialize(recipe);
            }
        }

        [Button]
        public void SetActiveCategory(RecipeCategory category)
        {
            if (!categoryPanels.ContainsKey(category)) return;

            activeCategory = category;

            foreach (var cat in categoryPanels.Keys)
            {
                UIHelper.ToggleCanvasGroup(categoryPanels[cat].GetComponent<CanvasGroup>(), cat == category, true, 0.3f);
            }

            // ✅ **Update ScrollRect Content**
            if (scrollRect != null && categoryPanels.ContainsKey(category))
            {
                scrollRect.content = categoryPanels[category].GetComponent<RectTransform>();
            }
        }

        public void Subscribe()
        {
            CoralIslandEvent.OnChangeRecipeCategory += SetActiveCategory;
        }

        public void Unsubscribe()
        {
            CoralIslandEvent.OnChangeRecipeCategory -= SetActiveCategory;
        }
    }
}
