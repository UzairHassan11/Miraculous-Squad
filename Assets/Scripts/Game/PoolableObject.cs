using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public PoolableObjectType poolableObjectType;

    public void ReturnToThePool()
    {
        PoolManager.instance.ReturnObject(gameObject, poolableObjectType);
    }
}