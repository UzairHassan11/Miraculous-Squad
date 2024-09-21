using UnityEngine;

public class EnemySpell : EnemyBullet
{
    [SerializeField] PoolableObject poolableObject;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isShooted = false;
            PlayerManager.instance.TakeDamage(damage);
            //ParticlesController.instance.SpawnParticle(PoolableObjectType.PlayerBulletHit, transform.position);
            gameObject.SetActive(false);
            poolableObject.ReturnToThePool();
            ParticlesController.instance.SpawnParticle(PoolableObjectType.EnemySpellHit, transform.position);
        }
        else
        if (other.CompareTag("Enemy"))
        { }
        else
        {
            isShooted = false;
            gameObject.SetActive(false);
            poolableObject.ReturnToThePool();
            ParticlesController.instance.SpawnParticle(PoolableObjectType.EnemySpellHit, transform.position);
        }
    }


    Transform mTransform;

    [SerializeField] float moveSpeed = 10;

    bool isShooted;

    // Start is called before the first frame update
    public void Init()
    {
        mTransform = transform;
        damage = 1; // assign damage here according to the level of the bullet
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooted)
            mTransform.position += mTransform.forward * Time.deltaTime * moveSpeed;
    }

    public void Shoot(Transform bulletSpawnPosition, bool lookTowardsPlayer = true)
    {
        if (mTransform == null)
            Init();

        //mTransform.LookAt(PlayerManager.instance.shootingController.bulletHitPosition);
        mTransform.position = bulletSpawnPosition.position;

        if(lookTowardsPlayer)
           mTransform.LookTowards(PlayerManager.instance.shootingController.bulletHitPosition);

        isShooted = true;
        gameObject.SetActive(true);
    }
}