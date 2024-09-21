using UnityEngine;

public static class PlayerPrefsContainer
{
    public static int CurrentEnvironment
    {
        get {return PlayerPrefs.GetInt("CurrentEnvironment", 0); }
        set {PlayerPrefs.SetInt("CurrentEnvironment", value); }
    }

    public static int CurrentLevel
    {
        get { return PlayerPrefs.GetInt("CurrentLevel", 0); }
        set { PlayerPrefs.SetInt("CurrentLevel", value); }
    }

    public static int PlayerLevel
    {
        get { return PlayerPrefs.GetInt("PlayerLevel", 0); }
        set { PlayerPrefs.SetInt("PlayerLevel", value); }
    }

    public static float PlayerCoins
    {
        get { return PlayerPrefs.GetFloat("PlayerCoins", 0); }
        set { PlayerPrefs.SetFloat("PlayerCoins", value); }
    }

    public static float PlayerGems
    {
        get { return PlayerPrefs.GetFloat("PlayerDiamonds", 0); }
        set { PlayerPrefs.SetFloat("PlayerDiamonds", value); }
    }

    public static int SelectedPowerUps
    {
        get { return PlayerPrefs.GetInt("SelectedPowerUps", -1); }
        set { PlayerPrefs.SetInt("SelectedPowerUps", value); }
    }

    [ContextMenu("ClearPrefs")]
    public static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}