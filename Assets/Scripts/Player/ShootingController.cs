using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;

public class ShootingController : MonoBehaviour
{
    #region variables
    [SerializeField] float shootDelay;
    float originalShootDelay;
    [ReadOnly]
    [SerializeField] float currentShootDelay;

    bool attack;

    [SerializeField] Vector2 minMaxAttackRange;

    [SerializeField]
    float attackRange;

    [SerializeField]
    ShootingRangeVisualiser shootingRangeVisualiser;

    public Transform bulletSpawnPosition, bulletHitPosition;

    VariableJoystick joystickControl;

    TriggeredEnemies triggeredEnemies;

    Queue<PlayerBullet> bullets = new Queue<PlayerBullet>();

    [SerializeField] int initialBulletsCount;

    [SerializeField] PlayerBullet bulletPrefab;
    PlayerController playerController;
    #endregion

    private void Start()
    {
        joystickControl = PlayerManager.instance.playerController.joystickControl;
        triggeredEnemies = PlayerManager.instance.enemiesTrigger;
        playerController = PlayerManager.instance.playerController;
        originalShootDelay = shootDelay;
        //InitBullets();
    }

    public void ResetAll()
    {
        //shootDelay = PlayerManager.instance.powerUpsController.GetCurrentValue(1);
    }

    private void Update()
    {
        if(GameManager.instance.currentGameState != GameState.Running)
            return;

        if(attack)
        {
            if (currentShootDelay <= 0)
            {
                if (joystickControl.isFree && triggeredEnemies.enemiesExist)
                {
                    ShootNow();
                }
            }
            else
            {
                currentShootDelay -= Time.deltaTime;
            }
        }
    }

    void ShootNow()
    {
        currentShootDelay = shootDelay;

        ShootBullet();
    }

    public void EnableShoot(bool state)
    {
        attack = state;
        if(!attack)
        {
            currentShootDelay = 0;
        }
    }

    /// <summary>
    /// change the attack range ranging from 0 - 1
    /// </summary>
    /// <param name="t">0-1 0 means minimum, 1 means maximum</param>
    public void ChangeRange(float t)
    {
        attackRange = Mathf.Lerp(minMaxAttackRange.x, minMaxAttackRange.y, t);
        shootingRangeVisualiser.ChangeScale(t);
    }

    //void InitBullets()
    //{
    //    PlayerBullet spawnedBullet;
    //    for (int i = 0; i < initialBulletsCount; i++)
    //    {
    //        spawnedBullet = Instantiate(bulletPrefab, transform);
    //        spawnedBullet.Init();
    //        bullets.Enqueue(spawnedBullet);
    //    }
    //}

    //void SpawnOneBullet()
    //{
    //    PlayerBullet spawnedBullet = Instantiate(bulletPrefab, transform);
    //    spawnedBullet.Init();
    //    bullets.Enqueue(spawnedBullet);
    //}
    Enemy closestEnemy;
    void ShootBullet()
    {
        //if (bullets.Count == 0)
        //    SpawnOneBullet();

        closestEnemy = triggeredEnemies.ClosestEnemy(transform.position);

        if(closestEnemy == null)
            return;

        // check if the closestEnemy is in range of attack accoding to the current level of player-bullets
        PlayerBullet bullet = PoolManager.instance.GetObject(PoolableObjectType.PlayerBullet).GetComponent<PlayerBullet>();
        bullet.Shoot(closestEnemy.transform);
        playerController.LookTowards(closestEnemy.transform);
        playerController.PlayShoot();
    }

    public void ReturnBullet(PlayerBullet bullet) => bullets.Enqueue(bullet);

    public void ChangeDemoFireRate()
    {
        ChangeFireRate(shootDelay / 2, 3);
    }

    public void ChangeFireRate(float changedFireRate, float time)
    {
        shootDelay = changedFireRate;
        StartCoroutine(ResetFireRate(time));
    }

    IEnumerator ResetFireRate(float t)
    {
        yield return new WaitForSeconds(t);
        shootDelay = originalShootDelay;
    }
}