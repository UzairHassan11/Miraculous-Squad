using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class PlayerBullet : MonoBehaviour
{
    Transform bulletPosition, mTransform;

    [SerializeField] float maxDistance = 20, moveSpeed = 10;

    bool isShooted, returning;
    
    public float damage = 1;
    [SerializeField]
    float scaleMultipleOnHit = 1.1f;

    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField] PoolableObject poolableObject;

    bool inited;
    [SerializeField]
    bool animate = true;
    float scale;

    // Start is called before the first frame update
    void Init()
    {
        bulletPosition = PlayerManager.instance.shootingController.bulletSpawnPosition;
        mTransform = transform;
        inited = true;
        scale = transform.localScale.x;
        //damage = 1; // assign damage here according to the level of the bullet
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooted)
        {
            if (!returning)
            {
                if (Vector3.Distance(mTransform.position, bulletPosition.position) > maxDistance || returning)
                    returning = true;
            }
            else
            {
                mTransform.LookAt(bulletPosition);
                if (Vector3.Distance(mTransform.position, bulletPosition.position) <= 0.5f)
                    ReturnToPool();
            }

            //mTransform.Translate(mTransform.forward * moveSpeed);
            mTransform.position += mTransform.forward * Time.deltaTime * moveSpeed;

            if(lineRenderer)
            {
                lineRenderer.SetPosition(0, bulletPosition.position);
                lineRenderer.SetPosition(1, mTransform.position);
            }
        }
    }
    void ReturnToPool()
    {
        returning = false;
        isShooted = false;
        gameObject.SetActive(false);
        if(animate)
            if(scaleAnim.IsPlaying())
                scaleAnim.Kill(true);
        //mTransform.localEulerAngles = Vector3.zero;
        //PlayerManager.instance.shootingController.ReturnBullet(this);
        poolableObject.ReturnToThePool();
    }

    Tween scaleAnim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            returning = true;
            Enemy enemy = other.GetComponent<Enemy>();
            if (!enemy.Triggered)
                enemy.Trigger();
            enemy.TakeDamage(damage);
            SoundManager.Instance.PlaySound(ClipName.EnemyHit);
            //transform.MultiplyLocalScale(scaleMultipleOnHit);
            if(animate)
                scaleAnim = transform.DOScale(scaleMultipleOnHit, .1f).SetRelative(true).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
            ParticlesController.instance.SpawnParticle(PoolableObjectType.PlayerBulletHit, transform.position);
            if(SaveData.Instance.Haptic)
                    MMVibrationManager.Haptic(HapticTypes.SoftImpact);
        }
        //else if (other.CompareTag("Player"))
        //{
        //    ReturnedToPlayer();
        //}
    }

    public void Shoot(Transform target)
    {
        if (!inited)
            Init();

        bulletPosition = PlayerManager.instance.shootingController.bulletSpawnPosition;
        mTransform.position = bulletPosition.position;

        mTransform.LookTowards(target);
        isShooted = true;
        //mTransform.parent = null;
        //Debug.Break();
        //print(target.name);
        gameObject.SetActive(true);
        //if (usedBefore)
        //    Debug.Break();
    }

}