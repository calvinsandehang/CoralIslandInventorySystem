using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemInfo : MonoBehaviour, ISubscriber
{
    [SerializeField] private Image recipeImage;
    [SerializeField] private TextMeshProUGUI tmpRecipeName;
    [SerializeField] private TextMeshProUGUI tmpItemCategory;
    [SerializeField] private TextMeshProUGUI tmpItemDescription;
    [SerializeField] private List<GameObject> toggledObjects = new();

    [Header("Item Requirement")]
    [SerializeField] private GameObject itemReqPrefab;
    [SerializeField] private Transform itemReqSpawnPoint;

    private List<GameObject> activeRequirementObjects = new List<GameObject>();

    private void Start()
    {
        ResetUI();
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
        ClearRequirements();
    }

    public void Initialize(CraftingRecipeSO recipeSO)
    {
        if (recipeSO == null || !recipeSO.IsDiscovered)
        {
            ResetUI();
            return;
        }

        SetToggledObjectsActive(true);

        recipeImage.sprite = recipeSO.ItemSprite;
        recipeImage.gameObject.SetActive(recipeSO.ItemSprite != null);

        tmpRecipeName.text = recipeSO.ItemName;
        tmpItemCategory.text = recipeSO.Category.ToString();
        tmpItemDescription.text = recipeSO.ItemDescription;

        PopulateRequirements(recipeSO.ItemRequirements);
    }

    private void PopulateRequirements(List<ItemRequirement> requirements)
    {
        ClearRequirements();

        if (itemReqSpawnPoint == null)
        {
            Debug.LogError("[UIItemInfo] Item requirement spawn point is missing.");
            return;
        }

        foreach (var req in requirements)
        {
            if (itemReqPrefab == null)
            {
                Debug.LogError("[UIItemInfo] itemReqPrefab is not assigned!");
                continue;
            }

            GameObject reqObject = PoolingManager.Instance.GetFromPool(itemReqPrefab, itemReqSpawnPoint);
            if (reqObject == null)
            {
                Debug.LogError($"[UIItemInfo] Failed to spawn {itemReqPrefab.name}");
                continue;
            }

            reqObject.transform.SetParent(itemReqSpawnPoint, false);
            reqObject.SetActive(true);

            UIItemRequirement itemRequirementUI = reqObject.GetComponent<UIItemRequirement>();
            if (itemRequirementUI != null)
            {
                itemRequirementUI.Initialize(req.AmountNeeded, req.Item);
            }
            else
            {
                Debug.LogError("[UIItemInfo] UIItemRequirement component is missing on the spawned object.");
            }

            activeRequirementObjects.Add(reqObject);
        }

        Debug.Log($"[UIItemInfo] Successfully spawned {activeRequirementObjects.Count} item requirements.");
    }

    private void ClearRequirements()
    {
        foreach (var reqObject in activeRequirementObjects)
        {
            if (reqObject != null)
            {
                PoolingManager.Instance.ReturnToPool(reqObject);
            }
        }
        activeRequirementObjects.Clear();
    }

    private void ResetUI()
    {
        SetToggledObjectsActive(false);

        if (recipeImage != null)
        {
            recipeImage.sprite = null;
            recipeImage.gameObject.SetActive(false);
        }

        if (tmpRecipeName != null) tmpRecipeName.text = "";
        if (tmpItemCategory != null) tmpItemCategory.text = "";
        if (tmpItemDescription != null) tmpItemDescription.text = "";

        ClearRequirements();
    }

    private void SetToggledObjectsActive(bool isActive)
    {
        for (int i = 0; i < toggledObjects.Count; i++)
        {
            if (toggledObjects[i] != null)
                toggledObjects[i].SetActive(isActive);
        }
    }

    public void Subscribe()
    {
        CoralIslandEvent.OnRecipeChosen += Initialize;
    }

    public void Unsubscribe()
    {
        CoralIslandEvent.OnRecipeChosen -= Initialize;
    }
}
