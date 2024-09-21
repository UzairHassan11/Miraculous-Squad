// Build Maker
// For making iOS and Android builds automatically at default paths outside project folder
// Default Path for iOS build is /Users/${UserName}/Documents/_Builds/${Product Name}/iOS Build
// Default Path for Android build is ${UserName}/Documents/_Builds/${Product Name}/Android Build
// Only active scenes in build settings are included in build
// No need to create any folders all folders will be created automatically
// If build already exists no need to delete, it will Append existing build for iOS
// If APK already exists with the same version it will be deleted and new build will be generated
// If build is completed successfully respective folders are automatically opened
// Shortcut key for Generating iOS Build => Cmd + Shift + i
// Shortcut key opening Build Path       => Cmd + Shift + o

using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
#endif

public class BuildMaker
{
    #if UNITY_EDITOR

    // Uncomment this function and its call inside MakeBuild Functions after importing Game Analytics into the Project

    // private static void SetGameAnalyticsVersion()
    // {
    //     if(GameAnalyticsSDK.GameAnalytics.SettingsGA)
    //     {
    //         for(int i=0 ; i<GameAnalyticsSDK.GameAnalytics.SettingsGA.Build.Count ; i++)
    //         {
    //             GameAnalyticsSDK.GameAnalytics.SettingsGA.Build[i] = PlayerSettings.bundleVersion;
    //         }//loop end
    //     }//if end
    // }//SetGameAnalyticsVersion() end

    private static string GetBuildPath(BuildTarget target)
    {
        string path  = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //One check only for MacOSX because on MAC the Documents folder is inside the path returned by the above statement
        if(SystemInfo.operatingSystemFamily.Equals(OperatingSystemFamily.MacOSX))
            path += "/Documents";

        if(!Directory.Exists(path + "/_Builds"))
            Directory.CreateDirectory(path + "/_Builds");
        if(!Directory.Exists(path + "/_Builds/" + PlayerSettings.productName))
            Directory.CreateDirectory(path + "/_Builds/" + PlayerSettings.productName);
        path += "/_Builds/" + PlayerSettings.productName;

        switch(target)
        {
            case BuildTarget.iOS:
                if(!Directory.Exists(path + "/iOS Build"))
                    Directory.CreateDirectory(path + "/iOS Build");
                path += "/iOS Build";
            break;
            case BuildTarget.Android:
                if(!Directory.Exists(path + "/Android Build"))
                    Directory.CreateDirectory(path + "/Android Build");
                path += "/Android Build";
            break;
        }//switch end
        return path;
    }//GetBuildPath() end

    private static string[] GetActiveScenes()
    {
        List<string> scenes = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if(scene.enabled)
                scenes.Add(scene.path);
        }//loop end
        return scenes.ToArray();
    }//GetBuildScenes() end

    private static void OpenInExplorer(string path) => EditorUtility.RevealInFinder(path);

    [MenuItem("Utils/Generate Android Build")]
    private static void GenerateAndroidBuild()
    {
        if(UnityEditor.EditorUtility.DisplayDialog("BUILD MAKER", "Do you want to Generate Android Build?\n\nWARNING\nIf your project is not on Android platform it will be switched.", "Yes", "No"))
            MakeAndroidBuild();
    }//GenerateAppleBuild() end

    private static void MakeAndroidBuild()
    {
        string BuildPath = GetBuildPath(BuildTarget.Android);
        string version = System.DateTime.Now.ToString("y.M.d");
        string BuildName = BuildPath + "/" + PlayerSettings.productName + " v" + version + ".apk";
        // SetGameAnalyticsVersion();

        //if same version apk exists then delete it
        if(File.Exists(BuildName))  
            File.Delete(BuildName);

        // PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, Android.BundleID);
        PlayerSettings.bundleVersion             = version;
        PlayerSettings.Android.bundleVersionCode = 1;
        PlayerSettings.Android.minSdkVersion     = AndroidSdkVersions.AndroidApiLevel22;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes             = GetActiveScenes();
        buildPlayerOptions.locationPathName   = BuildName;
        buildPlayerOptions.target             = BuildTarget.Android;
        buildPlayerOptions.options            = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded)
        {
            OpenInExplorer(BuildName);
            Debug.Log("Build Generated Successfully for Android Platform to path\n" + BuildPath);
        }//if end
    }//MakeAndroidBuild() end

    //Shortcut key is Ctrl + Shift + i
    [MenuItem("Utils/Generate iOS Build %#i")]
    private static void GenerateAppleBuild()
    {
        if(UnityEditor.EditorUtility.DisplayDialog("BUILD MAKER", "Do you want to Generate Apple Build?\n\nWARNING\nIf your project is not currently on iOS Platform it will be switched.", "Yes", "No"))
            MakeiOSBuild();
    }//GenerateAppleBuild() end

    private static void MakeiOSBuild()
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        string BuildPath = GetBuildPath(BuildTarget.iOS);
        // SetGameAnalyticsVersion();

        PlayerSettings.bundleVersion   = System.DateTime.Now.ToString("y.M.d");
        PlayerSettings.iOS.buildNumber = "1";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes             = GetActiveScenes();
        buildPlayerOptions.locationPathName   = BuildPath;
        buildPlayerOptions.target             = BuildTarget.iOS;
        //This will append build for iOS if already build exists
        if(File.Exists(Path.Combine(BuildPath, "Info.plist")))
            buildPlayerOptions.options = BuildOptions.AcceptExternalModificationsToPlayer;
        else
            buildPlayerOptions.options = BuildOptions.None;

        BuildReport report   = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded)
        {
            OpenBuildPath();
            Debug.Log("Build Generated Successfully for iOS Platform to path\n" + BuildPath);
        }//if end
    }//GenerateAppleBuild() end

    //BuildPath will be opened based on the Platform you project currently is
    [MenuItem("Utils/OpenBuildPath %#o")]
    private static void OpenBuildPath()
    {
        string BuildPath = string.Empty;
        switch(EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.iOS:
                BuildPath = GetBuildPath(BuildTarget.iOS);
                if(File.Exists(Path.Combine(BuildPath, "Info.plist")))
                {
                    // OpenInExplorer(BuildPath + "/Unity-iPhone.xcworkspace"); //if workspace then open to workspace
                    OpenInExplorer(BuildPath + "/Unity-iPhone.xcodeproj");   //else if xcodeproj found then open to xcodeproj
                    return;
                }//if end
            break;
            case BuildTarget.Android:
                BuildPath = GetBuildPath(BuildTarget.Android);
                if(File.Exists(BuildPath + "/" + PlayerSettings.productName + " v" + System.DateTime.Now.ToString("y.M.d") + ".apk"))
                {
                    OpenInExplorer(BuildPath + "/" + PlayerSettings.productName + " v" + System.DateTime.Now.ToString("y.M.d") + ".apk");
                    return;
                }//if end
            break;
        }//switch end
        OpenInExplorer(BuildPath); 
    }//OpenBuildPath() end

    #endif

}//class end