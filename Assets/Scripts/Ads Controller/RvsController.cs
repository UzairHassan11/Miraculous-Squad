using UnityEngine;
using UnityEngine.UI;

public class RvsController : MonoBehaviour
{

    public Button autoTapButton;
    public GameObject autoTapButtonOverlay;
    public Image autoTapButtonOverlayImage;

    private float AutoTapTimer
    {
        set => PlayerPrefs.SetFloat("AutoTapTimer",value);
        get => PlayerPrefs.GetFloat("AutoTapTimer",0);
    }
    
    
    public Button incomeX2Button;
    public GameObject incomeX2ButtonOverlay;
    public Image incomeX2ButtonOverlayImage;

    private float IncomeX2Timer
    {
        set => PlayerPrefs.SetFloat("IncomeX2Timer",value);
        get => PlayerPrefs.GetFloat("IncomeX2Timer",0);
    }



    private void Update()
    {
        autoTapButton.gameObject.SetActive(PlayerPrefsContainer.CurrentEnvironment == 0);
        incomeX2Button.gameObject.SetActive(PlayerPrefsContainer.CurrentEnvironment == 0);
        
        autoTapButton.interactable = (AdController.Instance.IsRewardedAdAvailable() && AutoTapTimer <= 0);
        incomeX2Button.interactable = (AdController.Instance.IsRewardedAdAvailable() && IncomeX2Timer <= 0);

        if (AutoTapTimer > 0)
            AutoTapTimer -= Time.deltaTime;
        else
            AutoTapTimer = 0;
        
        autoTapButtonOverlay.SetActive(AutoTapTimer > 0);
        autoTapButtonOverlayImage.fillAmount = Mathf.Lerp(1, 0, Mathf.InverseLerp(60, 0, AutoTapTimer));
        
        if (IncomeX2Timer > 0)
            IncomeX2Timer -= Time.deltaTime;
        else
            IncomeX2Timer = 0;

        incomeX2ButtonOverlay.SetActive(IncomeX2Timer > 0);
        incomeX2ButtonOverlayImage.fillAmount = Mathf.Lerp(1, 0, Mathf.InverseLerp(60, 0, IncomeX2Timer));
    }


    public void OnClickAutoTapButton()
    {
        AdController.Instance.ShowRewardedAd((isRewarded) =>
        {
            if(isRewarded)
                AutoTapTimer = 60;
        });
    }

    public void OnClickIncomeX2Button()
    {
        AdController.Instance.ShowRewardedAd((isRewarded) =>
        {
            if(isRewarded)
                IncomeX2Timer = 60;
        });
    }
}
