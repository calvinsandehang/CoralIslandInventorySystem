using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class PopUpUIManager : MonoBehaviour
{
    public static PopUpUIManager Instance { get; private set; }

    [Header("Popup Database")]
    [SerializeField, InlineEditor] private PopupDatabase popupDatabase;

    private Dictionary<PopupType, Queue<GameObject>> popupPools = new Dictionary<PopupType, Queue<GameObject>>();
    private Canvas mainCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        mainCanvas = FindObjectOfType<Canvas>(); // Find the main Canvas
        if (mainCanvas == null)
        {
            Debug.LogError("[PopUpUIManager] No Canvas found in the scene.");
        }

        InitializePools();
    }

    private void InitializePools()
    {
        foreach (PopupType type in System.Enum.GetValues(typeof(PopupType)))
        {
            popupPools[type] = new Queue<GameObject>();
        }
    }

    /// <summary>
    /// Spawns a popup at the target Transform's position and moves it under the canvas hierarchy.
    /// </summary>
    public GameObject ShowPopup(PopupType type, Transform targetTransform, string message, float xOffset = 0, float yOffset = 0)
    {
        if (mainCanvas == null) return null;

        GameObject popup = GetPopupFromPool(type);
        if (popup == null)
        {
            Debug.LogError($"[PopUpUIManager] Could not retrieve popup of type {type}");
            return null;
        }

        RectTransform popupRect = popup.GetComponent<RectTransform>();
        if (popupRect == null) return null;

        // Set popup under the main canvas
        popup.transform.SetParent(mainCanvas.transform, false);

        // Adjust position based on the target transform and apply offset
        Vector3 adjustedPosition = targetTransform.position + new Vector3(xOffset, yOffset, 0);
        popupRect.position = adjustedPosition;

        // Initialize text
        PopupUIController popupController = popup.GetComponent<PopupUIController>();
        if (popupController != null)
        {
            popupController.Initialize(message);
        }

        popup.SetActive(true);
        return popup;
    }


    /// <summary>
    /// Hides and returns the popup to the pool.
    /// </summary>
    public void HidePopup(GameObject popup, PopupType type)
    {
        popup.SetActive(false);
        popupPools[type].Enqueue(popup);
    }

    /// <summary>
    /// Retrieves a popup from the pool or instantiates a new one.
    /// </summary>
    private GameObject GetPopupFromPool(PopupType type)
    {
        if (popupPools[type].Count > 0)
        {
            return popupPools[type].Dequeue();
        }
        else
        {
            GameObject prefab = popupDatabase.GetPopupPrefab(type);
            if (prefab != null)
            {
                return Instantiate(prefab, Vector3.zero, Quaternion.identity, mainCanvas.transform);
            }
        }
        return null;
    }
}
