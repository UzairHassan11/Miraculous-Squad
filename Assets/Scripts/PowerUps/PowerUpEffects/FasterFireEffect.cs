using UnityEngine;

public class FasterFireEffect : PowerUpEffect
{
    public override void TurnOnEffect()
    {
        base.TurnOnEffect();
        PlayerManager.instance.shootingController.ChangeFireRate(data.GetCurrentValue, data.GetCurrentDuration);
    }
}