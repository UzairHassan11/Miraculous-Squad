using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;

public class FBAnalytics : MonoBehaviour
{
    public static FBAnalytics ins;
    // Start is called before the first frame update
    void Start()
    {
        if (!ins) ins = this;
        DontDestroyOnLoad(gameObject);
        
        
        try
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    InitializeFirebase();
                    InitializeFirebaseRemoteConfig();
                }
                else
                {
                    Debug.LogError(System.String.Format(
                   "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });
        }
        catch (System.Exception)
        {
        }

//        FetchDataAsync();


    }
    void InitializeFirebase()
    {
        try
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
            FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.ParameterCampaign, "Google");
            FirebaseAnalytics.SetSessionTimeoutDuration(new System.TimeSpan(0, 30, 0));
            Debug.Log("firebase init");
            
            //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

    }

    public void OnTutorialStartEvent(string tutorialName)
    {

        Debug.Log("TutorialStart_" + tutorialName);
        FirebaseAnalytics.LogEvent("TutorialStart_" + tutorialName);
        // try
        // {
        // }
        // catch (System.Exception)
        // {
        //     
        // }
    }

    public void OnTutorialCompletedEvent(string tutorialName)
    {
        Debug.Log("TutorialCompleted_" + tutorialName);
        FirebaseAnalytics.LogEvent("TutorialCompleted_" + tutorialName); 

        // try
        // {
        //
        // }
        // catch (System.Exception)
        // {
        // }
    }

    public void OnRaceStartEvent(int raceId)
    {
   FirebaseAnalytics.LogEvent("RaceStart_" + raceId.ToString());
   Debug.Log("RaceStart_" + raceId.ToString());
        // try
        // {
        //  
        // }
        // catch (System.Exception)
        // { }
    }

    public void OnRaceCompletedEvent(int raceId)
    {
        FirebaseAnalytics.LogEvent("RaceCompleted_" + raceId.ToString());
        Debug.Log("RaceCompleted_" + raceId.ToString());
        // try
        // {
        //     
        //    
        // }
        // catch (System.Exception)
        // {
        // }
    }
    
    public void OnRaceLostEvent(int raceId)
    {
        
        FirebaseAnalytics.LogEvent("RaceLost_" + raceId.ToString());
        Debug.Log("RaceLost_" + raceId.ToString());

        // try
        // {
        // }
        // catch (System.Exception)
        // {
        // }
    }
    
    
    public void OnUnlockingMergeBlock(int blockIndex)
    {
        FirebaseAnalytics.LogEvent("UnlockingMergeBlock_" + blockIndex.ToString());
        Debug.Log("UnlockingMergeBlock_" + blockIndex.ToString());
        // try
        // {
        // }
        // catch (System.Exception)
        // {
        // }
    }
    
    // public void OnUnlockingNewPart(int partIndex, ItemTypes.types myType)
    // {
    //     var type = "";
    //     switch (myType)
    //     {
    //         case ItemTypes.types.Wheel:
    //             type = "Wheel";
    //             break;
    //         case ItemTypes.types.Engine:
    //             type = "Engine";
    //             break;
    //         case ItemTypes.types.Body:
    //             type = "Body";
    //             break;
    //     }

    //     if (PlayerPrefs.GetInt($"UnlockingNewPart_{type}", -1) < partIndex)
    //     {
    //         FirebaseAnalytics.LogEvent($"UnlockingNewPart_{type}_" + partIndex.ToString());
    //         Debug.Log($"UnlockingNewPart_{type}_" + partIndex.ToString());
    //         PlayerPrefs.SetInt($"UnlockingNewPart_{type}", partIndex);
    //     }
    //     // try
    //     // {
    //     //    
    //     //     
    //     // }
    //     // catch (System.Exception)
    //     // {
    //     // }
    // }
    
    void InitializeFirebaseRemoteConfig() {
        // [START set_defaults]
        System.Collections.Generic.Dictionary<string, object> defaults =
            new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add("config_test_string", "default local string");
        defaults.Add("config_test_int", 1);
        defaults.Add("config_test_float", 1.0);
        defaults.Add("config_test_bool", false);

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task => {
                // [END set_defaults]
                Debug.Log("RemoteConfig configured and ready!");
                FetchDataAsync();
                
            });

    }
    
    // Start a fetch request.
// FetchAsync only fetches new data if the current data is older than the provided
// timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
// By default the timespan is 12 hours, and for production apps, this is a good
// number. For this example though, it's set to a timespan of zero, so that
// changes in the console will always show up immediately.
    public Task FetchDataAsync() {
        Debug.Log("Fetching data...");
        
        
        System.Threading.Tasks.Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);
        
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
        return fetchTask.ContinueWithOnMainThread(async task =>
        {
            await Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                
            var values = FirebaseRemoteConfig.DefaultInstance.AllValues;
            values.TryGetValue("interstitial_interval", out var configValue);
            RemoteInterstitialTimer = float.Parse(configValue.StringValue);
            // if(AdController.Instance)
            // {
            //     Debug.Log($"fldsnjfhsdfhjshfjksdhfjh.");
            //     AdController.Instance.ShowInterstitialAdWithTimer();
            // }   
            Debug.Log(configValue.StringValue);
        });
    }
    private float RemoteInterstitialTimer
    {
        set => PlayerPrefs.SetFloat("RemoteInterstitialTimer", value);
    }
    
    public static float TapTapTutorialTimer
    {
        set => PlayerPrefs.SetFloat("TapTapTutorialTimer", value);
        get => PlayerPrefs.GetFloat("TapTapTutorialTimer", 1);

    }

    
    public static bool IsTapTapTutorialOn
    {
        private set => PlayerPrefs.SetInt("IsTapTapTutorialOn", value ? 1 : 0);
        get => PlayerPrefs.GetInt("IsTapTapTutorialOn", 0) == 1;
    }

    public static bool IsIncomeTutorialOn
    {
        private set => PlayerPrefs.SetInt("IsIncomeTutorialOn", value ? 1 : 0);
        get => PlayerPrefs.GetInt("IsIncomeTutorialOn", 0) == 1;
    }

    
    private void FetchComplete(Task fetchTask) {
        if (!fetchTask.IsCompleted) {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if(info.LastFetchStatus != LastFetchStatus.Success) {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
            .ContinueWithOnMainThread(
                task =>
                {
                    Debug.Log("interstitial_interval    "+remoteConfig.GetValue("interstitial_interval").StringValue);

                    RemoteInterstitialTimer = float.Parse(remoteConfig.GetValue("interstitial_interval").StringValue);
                    TapTapTutorialTimer = float.Parse(remoteConfig.GetValue("tap_tap_tut_duration").StringValue);
                    
                    // RemoteInterstitialTimer = float.Parse(remoteConfig.GetValue("is_boost_income_tut_on").StringValue);
                    // RemoteInterstitialTimer = float.Parse(remoteConfig.GetValue("is_tap_tap_tut_on").StringValue);

                    IsTapTapTutorialOn = remoteConfig.GetValue("is_tap_tap_tut_on").BooleanValue;
                    IsIncomeTutorialOn = remoteConfig.GetValue("is_boost_income_tut_on").BooleanValue;
                    
                    print($"IsTapTapTutorialOn {IsTapTapTutorialOn}");
                    print($"IsIncomeTutorialOn {IsIncomeTutorialOn}");
                    // if(AdController.Instance)
                    // {
                    //     Debug.Log($"fldsnjfhsdfhjshfjksdhfjh.");
                    //     AdController.Instance.ShowInterstitialAdWithTimer();
                    // }   
                    Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                });
    }
    #region Firebase Messaging
    //public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    //{
    //    UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    //}

    //public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    //{
    //    UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    //}
    #endregion
}