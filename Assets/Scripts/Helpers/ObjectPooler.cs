using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Singleton;

    [Header("Settings")]
    public int amountToPool = 50;
    public Transform poolParent;
    [SerializeField] List<GameObject> bulletPool;

    [Header("Prefabs")]
    public GameObject bulletPrefab;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializePool(bulletPrefab, ref bulletPool);
    }

    private void InitializePool(GameObject prefab, ref List<GameObject> pool)
    {
        pool = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.name = $"{prefab.name}_pooled_{i}";
            obj.SetActive(false);
            obj.transform.SetParent(poolParent);
            pool.Add(obj);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
                return bulletPool[i];
        }
        return null;
    }

    public void ResetAllObjects()
    {
        foreach (GameObject go in bulletPool)
        {
            go.SetActive(false);
            go.transform.SetParent(null);
        }
    }
}
