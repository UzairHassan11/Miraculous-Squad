using UnityEngine;

public class SlashEffect : PowerUpEffect
{
    public void AttackWithSlash(Enemy enemy)
    {
        effectObject.gameObject.SetActive(true);
        effectObject.transform.position = enemy.transform.position + new Vector3(0, 1, 0);
        StartCoroutine(PerformTurnOffEffectAfter(button.data.GetCurrentReloadTime));
        StartCoroutine(PerformReloadFill(button.data.GetCurrentReloadTime));
        GameManager.instance.uiManager.PlaceSlashClaw(enemy.transform);
    }

    public override void TurnOnEffect()
    {
        if (reloading)
            return;
        SoundManager.Instance.PlaySound(ClipName.PowerUpActive);
        // PlayerManager.instance.enemiesTrigger.subTrigger.GetClosestEnemyWithoutTriggering();

        Enemy closestEnemy = GameManager.instance.enemiesManager.ClosestEnemy();
        if(closestEnemy != null)
            AttackWithSlash(closestEnemy);
    }

    public override void ResetAll()
    {
        base.ResetAll();
        effectObject.GetComponent<PlayerBullet>().damage = data.GetCurrentValue;
    }
}