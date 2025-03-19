using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupDatabase", menuName = "Game/UI/Popup Database")]
public class PopupDatabase : ScriptableObject
{
    [SerializeField] private List<PopupTypeEntry> popupEntries = new List<PopupTypeEntry>();
    private Dictionary<PopupType, GameObject> popupDictionary;

    private void OnEnable()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        popupDictionary = new Dictionary<PopupType, GameObject>();

        foreach (var entry in popupEntries)
        {
            if (!popupDictionary.ContainsKey(entry.popupType))
            {
                popupDictionary.Add(entry.popupType, entry.popupPrefab);
            }
        }
    }

    /// <summary>
    /// Gets the popup prefab associated with a PopupType.
    /// </summary>
    /// <param name="type">PopupType to retrieve</param>
    /// <returns>GameObject prefab</returns>
    public GameObject GetPopupPrefab(PopupType type)
    {
        if (popupDictionary.TryGetValue(type, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"[PopupDatabase] No prefab found for PopupType: {type}");
        return null;
    }
}
