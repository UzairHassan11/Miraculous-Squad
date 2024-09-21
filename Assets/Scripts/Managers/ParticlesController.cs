using System.Collections;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    //[SerializeField] private GameObject[] particlesObjects;

    Transform mTransform;

    PoolManager poolManager;

    #region singleton

    public static ParticlesController instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private void Start()
    {
        poolManager = PoolManager.instance;
    }

    public void SpawnParticle(PoolableObjectType particlesName, Vector3 pos, float spawnDelay = 0, float scaleMultiple = 1)
    {
        if (spawnDelay > 0)
        {
            StartCoroutine(spawnWithDelay(particlesName, pos, spawnDelay, scaleMultiple));
            return;
        }

        //GameObject obj = Instantiate(particlesObjects[(int)particlesName], pos, particlesObjects[(int)particlesName].transform.rotation, mTransform);
        GameObject obj = poolManager.GetObject(particlesName);
        obj.transform.position = pos;
        obj.transform.localScale *= scaleMultiple;
        obj.gameObject.SetActive(true);
    }

    IEnumerator spawnWithDelay(PoolableObjectType particlesName, Vector3 pos, float spawnDelay = 0, float scaleMultiple = 1)
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnParticle(particlesName, pos, 0, scaleMultiple);
    }
}