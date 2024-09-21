using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class RewardedAdController : MonoBehaviour
{
    public string adUnitId = "ca-app-pub-3940256099942544/5224354917";
    public Toggle toggle;

    private RewardedAd _rewardedAd;

    private float _rewardedRetryAttempt;
    public bool isRewardedLoaded;
    private Action<bool> _onRewarded;

    private void UpdateAdStatus(bool status)
    {
        isRewardedLoaded = status;
        if(toggle)
            toggle.isOn = status;
    }
    
    public void InitializeRewardedAds()
    {
        UpdateAdStatus(false);
        // Load the first RewardedAd
        LoadRewardedAd();
    }
    
    
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(adUnitId, adRequest,
            (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    UpdateAdStatus(false);
                    try
                    {
                        _rewardedRetryAttempt+= 0.1f;
                        double retryDelay = Math.Pow(2, _rewardedRetryAttempt);
                        // retryDelay = 0.5f;

                        Invoke("LoadRewardedAd", (float)retryDelay);
                    }           
                    catch
                    {
                        Debug.Log("Catch: OnRewardedAdLoadFailedEvent error occured");
                    }
                    
                    return;
                }
                UpdateAdStatus(true);
                // Reset retry attempt
                _rewardedRetryAttempt = 0;
                
                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());
                RegisterEventHandlers(ad);
                _rewardedAd = ad;
            });
    }
    
    public void ShowRewardedAd(Action<bool> onRewarded)
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
        _onRewarded = onRewarded;

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show(reward =>
            {
                SendRewardStatus(true);

                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        else
        {
            SendRewardStatus(false);
        }
    }
    private void SendRewardStatus(bool status)
    {
        _onRewarded?.Invoke(status);
        _onRewarded = null;
    }
    
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += adValue =>
        {
            UpdateAdStatus(false);

            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            UpdateAdStatus(false);

            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            UpdateAdStatus(false);

            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            UpdateAdStatus(false);

            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += error =>
        {
            UpdateAdStatus(false);
            LoadRewardedAd();
            SendRewardStatus(false);

            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}
