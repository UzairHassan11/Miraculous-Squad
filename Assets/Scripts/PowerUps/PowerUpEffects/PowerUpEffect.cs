using System.Collections;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
    [SerializeField] protected GameObject effectObject;

    protected PowerupData data;

    protected PowerUpButton button;

    protected bool reloading; 

    public virtual void Init(PowerUpButton _button)
    {
        button = _button;
        //_button.button.onClick.RemoveAllListeners();
        _button.button.onClick.AddListener(TurnOnEffect);
        data = button.data;
    }

    public virtual void TurnOnEffect()
    {
        if (reloading)
            return;

        if (effectObject)
            effectObject.SetActive(true);

        StartCoroutine(PerformTurnOffEffectAfter(button.data.GetCurrentDuration));
        StartCoroutine(PerformReloadFill(button.data.GetCurrentReloadTime));
        SoundManager.Instance.PlaySound(ClipName.PowerUpActive);
    }

    protected virtual IEnumerator PerformTurnOffEffectAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        if(effectObject)
            effectObject.SetActive(false);
    }

    protected virtual IEnumerator PerformReloadFill(float duration)
    {
        reloading = true;
        button.FillRloadImage(duration);
        yield return new WaitForSeconds(duration);
        reloading = false;
    }

    public virtual void ResetAll()
    {
        button.FillRloadImage(0);
        reloading = false;
        if (effectObject)
            effectObject.SetActive(false);
    }
}