using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleMobileAdsInitializer : MonoBehaviour
{
    public AdController adController;
    public void CustomInitialized()
    {
        adController.LoadNextScene();
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus =>
        {
            print(initStatus);
            print("Initialized ");
            adController.OnAdmobInitialized();
        });
    }
}
