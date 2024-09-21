using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas;
    [SerializeField] Image redImage, yellowImage;

    float animDuration = .2f;

    Tween tween, tween2;

    public void UpdateSlider(float fillAmount)
    {
        if (tween != null)
            if (tween.IsPlaying())
            {
                tween.Kill(true);
                //print("was playing it scale animation");
            }

        if (tween2 != null)
            if (tween2.IsPlaying())
            {
                tween2.Kill(true);
                //print("was playing it scale animation");
            }

        //if (fillAmount == 0)
        //    animDuration = 0;
        tween = redImage.DOFillAmount(fillAmount, fillAmount == 0 ? animDuration : 0).SetEase(Ease.Linear);
        tween2 = yellowImage.DOFillAmount(fillAmount, fillAmount == 0 ? animDuration : 0).SetEase(Ease.Linear).SetDelay(fillAmount == 0 ? animDuration : 0);
    }

    public void ShowHealthBar(bool state)
    {
        canvas.DOFade(state ? 1 : 0, .3f);
    }
}
