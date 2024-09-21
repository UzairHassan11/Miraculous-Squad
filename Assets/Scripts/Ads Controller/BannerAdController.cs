using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class BannerAdController : MonoBehaviour
{
    public Toggle toggle;

    public string adUnitId = "ca-app-pub-3940256099942544/6300978111";
 
    public AdPosition bannerPosition;
    
    private BannerView _bannerView;

    public bool IsBannerShowing{ get; private set; }
    private bool _isBannerLoaded;
    private float _bannerRetryAttempt;
    
    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void InitializeBannerAds()
    {
        // create an instance of a banner view first.
        if(_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        if (_bannerView != null) 
            _bannerView.LoadAd(adRequest);
    }

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(adUnitId, AdSize.Banner, bannerPosition);
        ListenToAdEvents();
    }

    private void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
            IsBannerShowing = false;

        }
    }
    public void ShowBanner()
    {
        if (_isBannerLoaded)
        {
            if(IsBannerShowing)
                return;
            IsBannerShowing = true;
            _bannerView.Show();
        }
        else
        {
            IsBannerShowing = false;
            InitializeBannerAds();

        }
    }

    public void HideBanner()
    {
        if (_bannerView != null)
        {
            IsBannerShowing = false;
            _bannerView.Hide();
            print("_bannerView != null");
        }
        else
        {
            print("_bannerView == null");
        }
    }
    

   
    
    /// <summary>
    /// listen to events the banner view may raise.
    /// </summary>
    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            _isBannerLoaded = true;
            if(toggle)
                toggle.isOn = true;
            ShowBanner();
            _bannerRetryAttempt = 0;
            Debug.Log("Banner view loaded an ad with response : "
                      + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += error =>
        {
            _isBannerLoaded = false;
            if(toggle)
                toggle.isOn = false;
            try
            {
                _bannerRetryAttempt += 0.1f;
                var retryDelay = Math.Pow(2, _bannerRetryAttempt);

                Invoke(nameof(InitializeBannerAds), (float)retryDelay);
            }           
            catch
            {
                Debug.Log("Catch: OnRewardedAdLoadFailedEvent error occured");
            }

            Debug.LogError("Banner view failed to load an ad with error : "
                           + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += adValue =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
}
