using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region singleton

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public PlayerController playerController;

    public ShootingController shootingController;

    public TriggeredEnemies enemiesTrigger;

    public HealthController healthController;

    public PowerUpsController powerUpsController;

    public CoinsCollector coinsCollector;


    public void TakeDamage(float damage)
    {
        if (healthController.OutOfHeath)
            return;

        if (powerUpsController.shieldMode)
        {
            SoundManager.Instance.PlaySound(ClipName.PlayerShieldHit);
            return;
        }
        healthController.TakeDamage(damage);

        if(healthController.OutOfHeath)
        {
            enemiesTrigger.RemoveAllEnemies();
            playerController.PlayDieAnim();
            shootingController.EnableShoot(false);
            healthController.ShowHealthBar(false);
            playerController.DieAndFail();
            GameManager.instance.ChangeGameState(GameState.Fail);
        }
        else
        {
            playerController.PlayDamageAnimation();
            SoundManager.Instance.PlaySound(ClipName.PlayerHit);
            //playerController.animationController.SetTrigger("GetHit");
        }
    }

    public void ResetAll()
    {
        enemiesTrigger.Enable(true);
        playerController.ResetAll();
        powerUpsController.Init();
        healthController.ResetHealth();
        coinsCollector.ResetAll();
        // shootingController.ResetAll();
    }

    public void Revive()
    {
        playerController.ResetAll();
        healthController.ResetHealth();
        playerController.PlaceMeAt(GameManager.instance.levelManager.spawnedSectionPlatform.spawnPosition);
    }
}