using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdController : MonoBehaviour
{
    public static AdController Instance;

    public Toggle toggle;
    public bool isInitialized;
    private void Awake()
    {
        Instance = this;
        isInitialized = false;
        _isMergeSceneLoaded = false;
        DontDestroyOnLoad(this);
    }

    private bool _isMergeSceneLoaded;
    public void LoadMergeScene()
    {
        if(_isMergeSceneLoaded)
            return;
        
        _isMergeSceneLoaded = true;
        
        SceneManager.LoadScene(mergeScene);
        Time.timeScale = 1;
    }

    [NaughtyAttributes.Scene]
    public string mergeScene;
    [SerializeField]
    private BannerAdController aLBanner;

    [SerializeField]
    private InterstitialAdController aLInterstitial;

    // [SerializeField]
    // public OpenAppAdController aLAppOpenAppImpl;

    [SerializeField]
    private RewardedAdController aLRewarded;

    void Start()
    {
        RestartInterstitialTimer();
    }  

    private void Update()
    {
        if(toggle)
            toggle.isOn = isInitialized;
        //  print("Interstitial Timer : " + aLInterstitial.interstitialTimer.ToString("F0"));
    }

    public void LoadNextScene()
    {
        Invoke(nameof(LoadMergeScene),4);
    }
    public void OnAdmobInitialized()
    {
        CancelInvoke();
        isInitialized = true;
        //aLAppOpenAppImpl.LoadAppOpenAd();
        // aLBanner.InitializeBannerAds();
        aLInterstitial.InitializeInterstitialAds();
        aLRewarded.InitializeRewardedAds();
        LoadMergeScene();
    }


    public bool IsRewardedAdAvailable()
    {
        // if (PlayerPrefsContainer.CurrentEnvironment == 0)
        //     return false;
        return aLRewarded.isRewardedLoaded;
    }
    public void ShowRewardedAd()
    {
        aLRewarded.ShowRewardedAd(null);
    }

    public void ShowRewardedAd(Action<bool> onRewarded)
    {
        if(IsRewardedAdAvailable())
        {
            aLRewarded.ShowRewardedAd(onRewarded);
            aLInterstitial.RestartInterstitialTimer();
        }
    }

    public void ShowBanner()
    {
        aLBanner.ShowBanner();
    }
    public void HideBanner()
    {
        aLBanner.HideBanner();
        print("Hide Banner");
    }

    public void ShowInterstitialAd()
    {
        aLInterstitial.ShowInterstitialAd();
    }

    public void ShowInterstitialAdWithTimer()
    {
        aLInterstitial.ShowInterstitialAdWithTimer();
    }

    public void CheckRemoteTimeAndShowInterstitialAd()
    {
        if(aLInterstitial.interstitialTimer <= 0)
        {
            ShowInterstitialAd();
            aLInterstitial.RestartInterstitialTimer();
        }
    }

    public void RestartInterstitialTimer()
    {
        aLInterstitial.RestartInterstitialTimer();
    }
}