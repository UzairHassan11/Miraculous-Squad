using UnityEngine;

[CreateAssetMenu(fileName = "Power-up Data", menuName = "ScriptableObjects/Power-Up-Data", order = 3)]
public class PowerupData : ScriptableObject
{
    public PowerUpType PowerUpType;

    public string description, valueName, valuePrefix, valuePostfix;

    public PowerUpLevel[] powerUpLevels;

    public Sprite unlockedIcon, lockedIcon;

    public string title, playerPref;

    public int unlockOnLevel = 2;

    private void OnValidate()
    {
        for (int i = 0; i < powerUpLevels.Length; i++)
        {
            powerUpLevels[i].index = i;
        }
    }

    public bool IsLocked => CurrentLevel < 0;

    public int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt(playerPref, -1);
        }
        set
        {
            // Debug.Log(value);
            PlayerPrefs.SetInt(playerPref, value);
        }
    }

    public int CurrentCards
    {
        get
        {
            return PlayerPrefs.GetInt(playerPref + "cards", 0);
        }
        set
        {
            // int n = CurrentLevel;
            // n++;
            // PlayerPrefs.SetInt(playerPref, n);
            PlayerPrefs.SetInt(playerPref + "cards", value);
        }
    }

    public float GetCurrentValue => powerUpLevels[CurrentLevel < 0 ? 0 : CurrentLevel].value;
    
    public float GetNextValue => powerUpLevels[CurrentLevel + 1].value;

    public float GetNextUpgradePrice => powerUpLevels[CurrentLevel + 1].price;

    public int RequiredCards => powerUpLevels[CurrentLevel + 1].cardsRequired;

    public bool HasReachedMax => CurrentLevel >= powerUpLevels.Length - 1;

    public bool HasRequiredCards => CurrentCards >= RequiredCards;

    public float GetCurrentDuration => powerUpLevels[curLvlIndex].duration;

    public float GetCurrentReloadTime => powerUpLevels[curLvlIndex].reloadTime;

    [ContextMenu("print current level")]
    void PrintCurrentLevel()
    {
        Debug.Log(CurrentLevel);
    }

    int curLvlIndex => CurrentLevel < powerUpLevels.Length ? CurrentLevel : powerUpLevels.Length - 1;

    public PowerUpLevel currentPowerUpLevel => powerUpLevels[curLvlIndex];
}
[System.Serializable]
public class PowerUpLevel
{
    public int index, cardsRequired = 5;
    public float price = 0, duration = 2, reloadTime = 5, value = 10;
}
public enum PowerUpType
{
    Slash,
    FireRate,
    Shield
}