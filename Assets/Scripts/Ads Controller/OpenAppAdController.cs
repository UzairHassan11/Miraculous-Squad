using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

public class OpenAppAdController : MonoBehaviour
{
    public string adUnitId = "ca-app-pub-3940256099942544/9257395921";

    private AppOpenAd _appOpenAd;

    private DateTime _expireTime;
    private float _openAppAdRetryAttempt;

    
    
    /// <summary>
    /// Loads the app open ad.
    /// </summary>
    public void LoadAppOpenAd()
    {
        return;
        
        // Clean up the old ad before loading a new one.
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(adUnitId, adRequest,
            (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    
                    try
                    {
                        _openAppAdRetryAttempt+= 0.1f;
                        var retryDelay = Math.Pow(2, _openAppAdRetryAttempt);
                        Invoke("LoadAppOpenAd", (float)retryDelay);
                    }
                    catch
                    {
                        // ignored
                    }
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());
                _openAppAdRetryAttempt = 0;

                // App open ads can be preloaded for up to 4 hours.
                _expireTime = DateTime.Now + TimeSpan.FromHours(0);
                
                _appOpenAd = ad;
                RegisterReloadHandler(ad);
                ShowAppOpenAd();

            });
    }
    
    
    
    private void Awake()
    {
        return;
        // Use the AppStateEventNotifier to listen to application open/close events.
        // This is used to launch the loaded ad when we open the app.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        return;
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    private bool IsAdAvailable
    {
        get
        {
            return _appOpenAd != null
                   && _appOpenAd.CanShowAd()
                   && DateTime.Now < _expireTime;
        }
    }
    private void OnAppStateChanged(AppState state)
    {
        return;
        Debug.Log("App State changed to : "+ state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (IsAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }
    
    /// <summary>
    /// Shows the app open ad.
    /// </summary>
    public void ShowAppOpenAd()
    {
        return;
        if (_appOpenAd != null && _appOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            _appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }
    
    private void RegisterReloadHandler(AppOpenAd ad)
    {
        return;
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += ()=>
        {
            Debug.Log("App open ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
        };
        
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += error =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
    }
}
