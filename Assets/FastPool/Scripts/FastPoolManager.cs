using UnityEngine;
using System.Collections.Generic;


public class FastPoolManager : MonoBehaviour
{
    public static FastPoolManager Instance
    { get; private set; }

    /// <summary>
    /// List of managed runtime pools
    /// </summary>
    public Dictionary<int, FastPool> Pools
    { get { return pools; } }

    [SerializeField]
    List<FastPool> predefinedPools;
    Dictionary<int, FastPool> pools;
    Transform curTransform;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            curTransform = GetComponent<Transform>();
            pools = new Dictionary<int, FastPool>();
        }
        else
            Debug.LogError("You cannot have more than one instance of FastPoolManager in the scene!");
    }

    void Start()
    {
        for (int i = 0; i < predefinedPools.Count; i++)
        {
            if (predefinedPools[i].Init(curTransform))
                pools.Add(predefinedPools[i].ID, predefinedPools[i]);
        }

        predefinedPools.Clear();
    }



    /// <summary>
    /// Create a new pool from provided component
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    /// <param name="component">Component which game object will be cloned</param>
    /// <param name="preloadCount">How much items must be preloaded and cached</param>
    /// <param name="capacity">Cache size (maximum amount of the cached items). [0 - unlimited]</param>
    /// <param name="warmOnLoad">Load source prefab in the memory while scene is loading. A bit slower scene loading, but much faster instantiating of new objects in pool</param>
    /// <returns>FastPool instance</returns>
    public static FastPool CreatePoolC<T>(T component, bool warmOnLoad = true, int preloadCount = 0, int capacity = 0)
        where T : Component
    {
        if (component != null)
            return CreatePool(component.gameObject, warmOnLoad, preloadCount, capacity);
        else
            return null;
    }

    /// <summary>
    /// Create a new pool from provided prefab
    /// </summary>
    /// <param name="prefab">Prefab that will be cloned</param>
    /// <param name="preloadCount">How much items must be preloaded and cached</param>
    /// <param name="capacity">Cache size (maximum amount of the cached items). [0 - unlimited]</param>
    /// <param name="warmOnLoad">Load source prefab in the memory while scene is loading. A bit slower scene loading, but much faster instantiating of new objects in pool</param>
    /// <returns>FastPool instance</returns>
    public static FastPool CreatePool(GameObject prefab, bool warmOnLoad = true, int preloadCount = 0, int capacity = 0)
    {
        if (prefab != null)
        {
            if (!Instance.pools.ContainsKey(prefab.GetInstanceID()))
            {
                FastPool newPool = new FastPool(prefab, Instance.curTransform, warmOnLoad, preloadCount, capacity);
                Instance.pools.Add(prefab.GetInstanceID(), newPool);
                return newPool;
            }
            else
                return Instance.pools[prefab.GetInstanceID()];
        }
        else
        {
            Debug.LogError("Creating pool with null object");
            return null;
        }
    }



    /// <summary>
    /// Returns pool for the specified prefab or creates it if needed (with default params)
    /// </summary>
    /// <param name="prefab">Source Prefab</param>
    /// <returns></returns>
    public static FastPool GetPool(GameObject prefab, bool createIfNotExists = true)
    {
        if (prefab != null)
        {
            if (Instance.pools.ContainsKey(prefab.GetInstanceID()))
                return Instance.pools[prefab.GetInstanceID()];
            else
                return CreatePool(prefab);
        }
        else
        {
            Debug.LogError("Trying to get pool for null object");
            return null;
        }
    }
    /// <summary>
    /// Returns pool for the specified prefab or creates it if needed (with default params)
    /// </summary>
    /// <param name="component">Any component of the source prefab</param>
    /// <returns></returns>
    public static FastPool GetPool(Component component, bool createIfNotExists = true)
    {
        if (component != null)
        {
            GameObject prefab = component.gameObject;
            if (Instance.pools.ContainsKey(prefab.GetInstanceID()))
                return Instance.pools[prefab.GetInstanceID()];
            else
                return CreatePool(prefab);
        }
        else
        {
            Debug.LogError("Trying to get pool for null object");
            return null;
        }
    }



    /// <summary>
    /// Destroys provided pool and it's cached objects
    /// </summary>
    /// <param name="pool">Pool to destroy</param>
    public static void DestroyPool(FastPool pool)
    {
        pool.ClearCache();
        Instance.pools.Remove(pool.ID);
    }
}
