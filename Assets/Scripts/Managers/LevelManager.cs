using UnityEngine;
using Firebase.Analytics;

public class LevelManager : MonoBehaviour
{
    int CurrentSectionNumber;

    public Level CurrentLevel;

    //LevelSelection levelSelection;

    LevelSection levelSection;

    [HideInInspector]
    public SectionPlatform spawnedSectionPlatform;

    [SerializeField] ExitIndicator exitPositionIndicator;

    PlayerController playerController;

    public LevelSection GetCurrentSection => levelSection;

    public Transform GetSectionEnemySpawnPoint(int i) => spawnedSectionPlatform.enemyPositionsContainer.GetChild(i);

    private void Start()
    {
        //levelSelection = GameManager.instance.levelSelection;
        playerController = PlayerManager.instance.playerController;
    }

    public void Init(Level level)
    {
        // print("1");
        CurrentSectionNumber = 0;
        CurrentLevel = level;
        InitSectionPlatform();
        GameManager.instance.uiManager.ShowFadePanel();
        TurnExitIndicator(false);
    }

    void InitSectionPlatform()
    {
        // FirebaseAnalytics.LogEvent("Started_Section_"+CurrentSectionNumber+"_Chapter_"
        //     +PlayerPrefsContainer.CurrentEnvironment
        //     +"_Level_"+PlayerPrefsContainer.CurrentLevel);
        levelSection = CurrentLevel.levelSections[CurrentSectionNumber];
        GameManager.instance.uiManager.IndicateSection(CurrentSectionNumber);
        //Invoke("SpawnSectionPlatform", .25f);
        
        //if (levelSection.startDialogue)
        //    DialogueController.instance.ShowDialogue(levelSection.startDialogue);
        SpawnSectionPlatform();
        // print("11");
    }

    public bool ShowStartDialogue()
    {
        if (levelSection.startDialogue == null)
            return false;
        else
        {
            DialogueController.instance.ShowDialogue(levelSection.startDialogue);
            levelSection.startDialogue.postDialogueActions += ()=> GameManager.instance.uiManager.ShowGameplayPanel(true);
            return true;
        }
    }

    public void TurnOffLastPlatform()
    {
        if (spawnedSectionPlatform)
        { 
            spawnedSectionPlatform.gameObject.SetActive(false);
            Destroy(spawnedSectionPlatform);
            PoolManager.instance.TurnOffAllObjects(PoolableObjectType.Coins);
        }
    }

    void SpawnSectionPlatform()
    {
        TurnOffLastPlatform();
        //PlayerManager.instance.playerController.ResetFirstTimeFall();
        // print("11111");
        spawnedSectionPlatform = Instantiate(levelSection.platform);

        // print("spawnedSectionPlatform.spawnEnemiesInStart " + spawnedSectionPlatform.spawnEnemiesInStart);
        if(spawnedSectionPlatform.spawnEnemiesInStart)
            GameManager.instance.enemiesManager.SpawnEnemiesForSection(levelSection);

        spawnedSectionPlatform.gameObject.SetActive(true);
        playerController.PlaceMeAt(spawnedSectionPlatform.spawnPosition);
        playerController.TurnMe(true);
        PlaceExitIndicator(spawnedSectionPlatform.exitPosition);
    }

    [ContextMenu("SectionCompleted")]
    public void SectionCompleted()
    {
        bool hasEndDialogue = false;
        GameManager.instance.levelSelection.SectionCompleted();
        PlayerManager.instance.healthController.ResetHealth();

        if (levelSection.endDialogue)
        {
            hasEndDialogue = true;
            DialogueController.instance.ShowDialogue(levelSection.endDialogue);
        }
        //print("SectionCompleted");
        if (CurrentSectionNumber == CurrentLevel.levelSections.Length - 1)
        {
            //level win
            if (hasEndDialogue)
                levelSection.endDialogue.postDialogueActions += () => GameManager.instance.ChangeGameState(GameState.Win);
            else
                GameManager.instance.ChangeGameState(GameState.Win);

            // FirebaseAnalytics.LogEvent("Cleared_Level_"+
            // PlayerPrefsContainer.CurrentLevel
            // +"_Chapter_"+PlayerPrefsContainer.CurrentEnvironment);
        }
        else
        {
            // FirebaseAnalytics.LogEvent("Cleared_Section_"+CurrentSectionNumber+"_Chapter_"
            // +PlayerPrefsContainer.CurrentEnvironment
            // +"_Level_"+PlayerPrefsContainer.CurrentLevel);
         
            CurrentSectionNumber++;
            if (hasEndDialogue)
                levelSection.endDialogue.postDialogueActions += () => InitSectionPlatform();
            else
                InitSectionPlatform();
        }

        PlayerManager.instance.powerUpsController.ResetInGamePowerUps();
    }

    public void LogFailAnylytics()
    {
        // FirebaseAnalytics.LogEvent("Failed_Section_"+CurrentSectionNumber+"_Chapter_"
        //     +PlayerPrefsContainer.CurrentEnvironment
        //     +"_Level_"+PlayerPrefsContainer.CurrentLevel);
    }

    public void PlaceExitIndicator(Transform t)
    {
        exitPositionIndicator.transform.position = t.position;
    }

    public void TurnExitIndicator(bool state)
    {
        exitPositionIndicator.SetVisibility(state);

        //if (CurrentLevel.levelSections.Length == 1)
        //{
        //    exitPositionIndicator.SetVisibility(state);
        //}
        //else
        //{
        //    exitPositionIndicator.DoTheThing();
        //}
    }

    public void ShowLevelRewards()
    {

        for (int i = 0; i < CurrentLevel.levelReward.Length; i++)
        {

        }
    }

    public Transform GetRandomSpawnPositionFromThisSection => spawnedSectionPlatform.RandomSpawnPositionFromThisSection;
}