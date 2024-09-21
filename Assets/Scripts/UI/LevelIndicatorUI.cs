using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelIndicatorUI : MonoBehaviour
{
    public Image fillImage;
    public Image destination;
    public Image rewardImage;

    public void FillImage(bool animate = false)
    {
        if (!fillImage)
            return;

        fillImage.DOFillAmount(1, animate ? 1 : 0).SetEase(Ease.Linear);
    }

    public void UnfillImage()
    {
        if (!fillImage)
            return;

        fillImage.fillAmount = 0;
    }
}