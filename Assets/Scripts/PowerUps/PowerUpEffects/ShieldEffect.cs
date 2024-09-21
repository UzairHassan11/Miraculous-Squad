using System.Collections;
using UnityEngine;

public class ShieldEffect : PowerUpEffect
{
    public override void TurnOnEffect()
    {
        base.TurnOnEffect();
        PlayerManager.instance.powerUpsController.shieldMode = true;
    }

    protected override IEnumerator PerformTurnOffEffectAfter(float duration)
    {
        reloading = true;
        yield return new WaitForSeconds(duration);
        if (effectObject)
            effectObject.SetActive(false);
        PlayerManager.instance.powerUpsController.shieldMode = false;
    }

    public override void ResetAll()
    {
        PlayerManager.instance.powerUpsController.shieldMode = false;
    }
}