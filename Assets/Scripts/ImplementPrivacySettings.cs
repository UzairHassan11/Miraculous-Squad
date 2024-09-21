using System;
using System.Collections.Generic;
// using GoogleMobileAds.Mediation.Chartboost.Api;
// using GoogleMobileAds.Mediation.AppLovin.Api;
// using GoogleMobileAds.Mediation.AdColony.Api;
// using GoogleMobileAds.Mediation.LiftoffMonetize.Api;
// using GoogleMobileAds.Mediation.UnityAds.Api;
// using GoogleMobileAds.Mediation.IronSource.Api;
// using GoogleMobileAds.Mediation.InMobi.Api;
using UnityEngine;


public class ImplementPrivacySettings : MonoBehaviour
{
    public GoogleMobileAdsInitializer googleMobileAdsInitializer;

    private void Start()
    {
        if (ConcentController.ConsentFormShown)
            Initialize();
    }

    public void Initialize()
    {
        // Chartboost.AddDataUseConsent(CBGDPRDataUseConsent.NonBehavioral);
        // Chartboost.AddDataUseConsent(CBCCPADataUseConsent.OptInSale);

        // AppLovin.SetHasUserConsent(true);
        // AppLovin.SetIsAgeRestrictedUser(false);
        // AppLovin.SetDoNotSell(true);

        // AdColonyAppOptions.SetPrivacyFrameworkRequired(AdColonyPrivacyFramework.GDPR, true);
        // AdColonyAppOptions.SetPrivacyConsentString(AdColonyPrivacyFramework.GDPR, "myPrivacyConsentString");
        // AdColonyAppOptions.SetPrivacyFrameworkRequired(AdColonyPrivacyFramework.CCPA, true);
        // AdColonyAppOptions.SetPrivacyConsentString(AdColonyPrivacyFramework.CCPA, "myPrivacyConsentString");

        // LiftoffMonetize.UpdateConsentStatus(VungleConsentStatus.OPTED_IN, "1.0.0");
        // LiftoffMonetize.UpdateCCPAStatus(VungleCCPAStatus.OPTED_IN);

        // UnityAds.SetConsentMetaData("gdpr.consent", true);
        // UnityAds.SetConsentMetaData("privacy.consent", true);

        // IronSource.SetConsent(true);
        // IronSource.SetMetaData("do_not_sell", "true");


        // InMobi.UpdateGDPRConsent(new Dictionary<string, string>
        // {
        //     { "gdpr_consent_available", "true" },
        //     { "gdpr", "1" }
        // });

        googleMobileAdsInitializer.CustomInitialized();
    }
}