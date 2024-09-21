using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region vars

    public GameState currentGameState;

    public UiManager uiManager;

    public LevelSelection levelSelection;

    public EnemiesManager enemiesManager;

    public LevelManager levelManager;

    #endregion

    #region singleton

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    #endregion

    #region others

    public void ChangeGameState(GameState requestedState)
    {
        if (currentGameState == GameState.Idle && requestedState == GameState.Running)
        {
            currentGameState = requestedState;
            CameraManager.instance.SetAnimatorState(CamStates.gameplay);
            PlayerManager.instance.powerUpsController.Init();
            PlayerManager.instance.ResetAll();
            levelManager.Init(levelSelection.CurrentLevel);
            SoundManager.Instance.PlayMainMenyBG(false);
        }
        //else if (currentGameState == GameState.Running &&
        //         requestedState == GameState.FinalMomentum)
        //{
        //    currentGameState = requestedState;
        //    CameraManager.instance.SetAnimatorState(CamStates.finalMomentum);
        //}
        else if (currentGameState == GameState.Running &&
                 requestedState == GameState.Win)
        {
            currentGameState = requestedState;
            uiManager.ShowWinPanel();
            levelSelection.IncreaseLevel();
            PlayerManager.instance.healthController.ResetHealth();
        }
        else if (currentGameState == GameState.Running &&
                 requestedState == GameState.Fail)
        {
            currentGameState = requestedState;
            uiManager.ShowFailPanel(1.5f);
            //CameraManager.instance.SetAnimatorState(CamStates.endPoint);
        }
        else if (currentGameState == GameState.Win &&
                 requestedState == GameState.Idle)
        {
            currentGameState = requestedState;
            levelSelection.UpdateIU();
            PlayerManager.instance.ResetAll();
        }
        else if (currentGameState == GameState.Fail &&
                 requestedState == GameState.Idle)
        {
            currentGameState = requestedState;
            levelSelection.UpdateIU();
            enemiesManager.DiableAllEnemies();
            PlayerManager.instance.ResetAll();
            levelManager.LogFailAnylytics();
        }else if (currentGameState == GameState.Running &&
                 requestedState == GameState.Idle)
        {
            currentGameState = requestedState;
        }
        else if (currentGameState == GameState.Fail &&
                 requestedState == GameState.Running)
        {
            currentGameState = requestedState;
        }
    }

    public void ChangeGameState(int index)
    {
        ChangeGameState((GameState) index);
    }
    #endregion
}

public enum GameState
{
    Idle,
    Running,
    FinalMomentum,
    Win,
    Fail
}