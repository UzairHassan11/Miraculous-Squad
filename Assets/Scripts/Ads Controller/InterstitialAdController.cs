using System;
using System.Collections;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterstitialAdController : MonoBehaviour
{
    public static Action OnInterShowing;
    public string adUnitId = "ca-app-pub-3940256099942544/1033173712";
    public Toggle toggle;
    public TMP_Text timer;

    private InterstitialAd _interstitialAd;

    private float _interstitialRetryAttempt;

    private Action _adFinishedCallback;

    public bool isInterstitialLoaded;

    public float interstitialTimer;

    private float RemoteInterstitialTimer => PlayerPrefs.GetFloat("RemoteInterstitialTimer", 10);


    public void InitializeInterstitialAds()
    {
        interstitialTimer = RemoteInterstitialTimer;
        
        UpdateAdStatus(false);
        
        // Load the first interstitial
        LoadInterstitialAd();
    }
    private void UpdateAdStatus(bool status)
    {
        isInterstitialLoaded = status;
        if(toggle)
            toggle.isOn = status;
    }
    
    
    
    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(adUnitId, adRequest,
            (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    UpdateAdStatus(false);

                    try
                    {
                        _interstitialRetryAttempt++;
                        var retryDelay = Math.Pow(2, _interstitialRetryAttempt);
                        Invoke(nameof(LoadInterstitialAd), (float)retryDelay);
                    }
                    catch
                    {
                        // ignored
                    }

                    return;
                }
                UpdateAdStatus(true);
                
                
                _interstitialRetryAttempt = 0;
                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());
                RegisterEventHandlers(ad);
                _interstitialAd = ad;

                if(_adRoutine != null)
                {
                    StopCoroutine(_adRoutine);
                    _adRoutine = null;
                }  
                
                interstitialTimer = RemoteInterstitialTimer;
                if(RemoteInterstitialTimer > 0)
                    _adRoutine = StartCoroutine(ShowInterstitialAdRoutine());
            });
    }

    private void Update()
    {
        if(timer)
            timer.text = interstitialTimer+"";

    }

    private Coroutine _adRoutine;
    
    IEnumerator ShowInterstitialAdRoutine()
    {
        yield break;
        while (interstitialTimer >= 0)
        {
            interstitialTimer -= Time.deltaTime;
            yield return null;
        }

        interstitialTimer = 0;
        ShowInterstitialAd();
        interstitialTimer = RemoteInterstitialTimer;
        
        if (RemoteInterstitialTimer > 0)
            _adRoutine = StartCoroutine(ShowInterstitialAdRoutine());
    }
    
    public void RestartInterstitialTimer()
    {
        StartCoroutine(_RestartInterstitialTimer());
    }

    IEnumerator _RestartInterstitialTimer()
    {
        print("_RestartInterstitialTimer");
        interstitialTimer = RemoteInterstitialTimer;

        while (interstitialTimer >= 0)
        {
            interstitialTimer -= Time.deltaTime;
            yield return null;
        }

        interstitialTimer = 0;
    }
    
    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        print("ShowInterstitialAd");
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            OnInterShowing?.Invoke();
            Invoke(nameof(ShowInterstitialAdWithDelay),0.5f);
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public void ShowInterstitialAdWithDelay()
    {
        if(RemoteInterstitialTimer < 0 || PlayerPrefsContainer.CurrentEnvironment == 0)
            return;
        
        Debug.Log("Showing interstitial ad.");
        _interstitialAd.Show();
    }
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += adValue =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            UpdateAdStatus(false);

            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd();

        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += error =>
        {
            UpdateAdStatus(false);

            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();

        };
    }

    public void ShowInterstitialAdWithTimer()
    {
        interstitialTimer = RemoteInterstitialTimer;
        if(isInterstitialLoaded == false)
        {
            LoadInterstitialAd();
            return;
        }        
        if(_adRoutine != null)
        {
            StopCoroutine(_adRoutine);
            _adRoutine = null;
        }  
                
        interstitialTimer = RemoteInterstitialTimer;
        if(RemoteInterstitialTimer > 0)
            _adRoutine = StartCoroutine(ShowInterstitialAdRoutine());
    }
}
