using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    #region singleton

    public static PoolManager instance;

    private void Awake()
    {
        instance = this;

        InitializePools();
    }

    #endregion

    [SerializeField] PoolableObjectType poolableObjectTypes;
    [SerializeField] PoolableObjectData[] pools;

    void InitializePools()
    {
        foreach (var item in pools)
        {
            item.Initialize();
        }
    }

    public GameObject GetObject(PoolableObjectType poolableObjectType)
    {
        return pools[(int)poolableObjectType].GetFromPool();
    }

    public void ReturnObject(GameObject obj, PoolableObjectType poolableObjectType)
    {
        pools[(int)poolableObjectType].AddToPool(obj);
        obj.transform.SetParent(pools[(int)poolableObjectType].container);
    }

    public void TurnOffAllObjects(PoolableObjectType type)
    {
        Transform container = pools[(int)type].container;
        for(int i = 0; i < container.childCount; i++)
        {
            if(container.GetChild(i).gameObject.activeSelf)
            {
                container.GetChild(i).gameObject.SetActive(false);
                container.GetChild(i).GetComponent<PoolableObject>().ReturnToThePool();
            }
        }
    }
}
[System.Serializable]
public class PoolableObjectData
{
    public GameObject prefab;
    public int initialSpawnCount, onEmptySpawnCount;
    public Queue<GameObject> objectsPool = new Queue<GameObject>();
    public Transform container;
    PoolableObjectType poolableObjectType;

    public void Initialize(bool start = true)
    {
        int spawnCount = start ? initialSpawnCount : onEmptySpawnCount;

        GameObject spawnedObject;

        for (int i = 0; i < spawnCount; i++)
        {
            spawnedObject = GameObject.Instantiate(prefab, container);
            AddToPool(spawnedObject);
        }

    }

    public void AddToPool(GameObject obj)
    {
        if(!objectsPool.Contains(obj))
            objectsPool.Enqueue(obj);
    }

    public GameObject GetFromPool()
    {
        if (objectsPool.Count == 0)
            Initialize(false);
        return objectsPool.Dequeue();
    }
}
public enum PoolableObjectType
{
    EnemyGrenade,
    EnemySpell,
    Coins,
    Diamonds,
    PlayerBullet,
    PlayerBulletHit,
    EnemyDead,
    SlashPowerUp,
    EnemySpawn,
    EnemySpellHit,
    EnemyGrenadeHit
}