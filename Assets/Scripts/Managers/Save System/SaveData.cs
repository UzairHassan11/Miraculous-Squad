//Introduce your new variables under Game Variables and pass them accordingly
//in the Constructor => SaveData() and Method => CreateSaveObject()

public class SaveData
{
    private static SaveData _Instance = null;

    public static SaveData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new SaveData();
                SaveSystem.LoadProgress();
            } //if end

            return _Instance;
        } //get end
    } //Property end

    public void Reset() => _Instance = new SaveData();

    #region all-vars
//Setting Variable
    public bool Music = true;
    public bool SFX = true;
    public bool Haptic = true;

    //Game Variables
    public int Level = 1;
    public int LevelToUse = 1;
    public int Coins = 100;
    public int LevelNumberPref = 1;
    public int LevelNumberAnalytics = 1;
    #endregion
    
    public int GA_Level = 0;

    public string HashOfSaveData;

    private SaveData()
    {
    }

    private SaveData(int level, int levelToUse, int coins, int ga_level, int levelNumberPref, int levelNumberAnalytics)
    {
        Level = level;
        LevelToUse = levelToUse;
        Coins = coins;
        GA_Level = ga_level;
        LevelNumberPref = levelNumberPref;
        LevelNumberAnalytics = levelNumberAnalytics;
    } //SaveData() end

    public SaveData CreateSaveObject() => new SaveData(Instance.Level, Instance.LevelToUse, Instance.Coins,
        Instance.GA_Level, Instance.LevelNumberPref, Instance.LevelNumberAnalytics);

} //class end