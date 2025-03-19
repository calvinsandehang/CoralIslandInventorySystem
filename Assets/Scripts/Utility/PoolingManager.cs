using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Retrieves an object from the pool or instantiates a new one if needed.
    /// </summary>
    public GameObject GetFromPool(GameObject prefab, Transform parent)
    {
        if (prefab == null)
        {
            Debug.LogError("[PoolingManager] Prefab is null!");
            return null;
        }

        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
            Debug.Log($"[PoolingManager] Creating new pool for {prefab.name}");
        }

        GameObject obj;
        if (poolDictionary[prefab].Count > 0)
        {
            obj = poolDictionary[prefab].Dequeue();
            if (obj == null) // Handle destroyed objects
            {
                Debug.LogWarning($"[PoolingManager] Re-spawning destroyed object for {prefab.name}");
                return GetFromPool(prefab, parent);
            }
        }
        else
        {
            obj = Instantiate(prefab);
            Debug.Log($"[PoolingManager] Instantiated new {prefab.name}");
        }

        if (parent != null && parent.gameObject.scene.IsValid())
        {
            obj.transform.SetParent(parent, false);
        }
        else
        {
            Debug.LogWarning($"[PoolingManager] Parent {parent} is not valid.");
        }

        obj.SetActive(true);
        return obj;
    }


    /// <summary>
    /// Returns an object to the pool.
    /// </summary>
    public void ReturnToPool(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("[PoolingManager] Tried to return a null object to the pool.");
            return;
        }

        obj.SetActive(false);

        foreach (var key in poolDictionary.Keys)
        {
            if (obj.name.StartsWith(key.name)) // Ensure we're returning to the correct pool
            {
                poolDictionary[key].Enqueue(obj);
                Debug.Log($"[PoolingManager] Returned {obj.name} to the pool.");
                return;
            }
        }

        Debug.LogWarning($"[PoolingManager] No pool found for object: {obj.name}");
    }

}
