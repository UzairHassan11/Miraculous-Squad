using UnityEngine;
using System.Collections;
using Firebase.Analytics;

public class TutorialController : MonoBehaviour
{
    #region singleton

    public static TutorialController instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (action1)
            if (Input.GetMouseButtonDown(0))
            {
                action1 = false;
                gameManager.uiManager.HideTutorialMsg();
                FirebaseAnalytics.LogEvent("Tutorial_Action1_Completed");
                // gameManager.uiManager.infinityPanel.onClickEvents += ()=>
                // gameManager.enemiesManager.SpawnEnemy(1, gameManager.levelManager.GetSectionEnemySpawnPoint(0).position, true, true);
            }

        // if (action2)
        // {
        //     if (Input.GetMouseButtonUp(0))
        //     {
        //         action2 = false;
        //         gameManager.uiManager.HideTutorialMsg();
        //     }
        // }

        // if (action3)
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         action3 = false;
        //         gameManager.uiManager.ShowTutorialMsg("Release finger to attack!");
        //     }
    }

    #region level1a
    bool action1, action2, action3;
    public void Action1()
    {
        gameManager.uiManager.ShowTutorialMsg("Use Joystick to move around");
        Invoke( "waitAndSetAction1True", .5f);
        FirebaseAnalytics.LogEvent("Tutorial_Action1_Started");

        return;
        gameManager.levelManager.GetCurrentSection.startDialogue.postDialogueActions += () => {
            gameManager.uiManager.ShowTutorialMsg("Use Joystick to move around");
            Invoke( "waitAndSetAction1True", .5f);
            // FirebaseAnalytics.LogEvent("Tutorial_Action1_Started");
            // action1 = true;
        };

    }
    void waitAndSetAction1True()
    {
            // StartCoroutine(WaitAndSetAction1True());
        action1 = true;

    }

    public void Tutorial1Actions()
    {
        gameManager.uiManager.ShowTutorialMsg("Release finger to attack!");
        // gameManager.uiManager.HideTutorialMsg();
        // gameManager.uiManager.infinityPanel.onClickEvents += ()=>
        gameManager.enemiesManager.SpawnEnemy(1, gameManager.levelManager.GetSectionEnemySpawnPoint(0).position, true, true);
        gameManager.enemiesManager.afterKillingAllSectionEnemiesEvents += ()=> gameManager.uiManager.HideTutorialMsg();
    } 
    IEnumerator WaitAndSetAction1True()
    {
        yield return new WaitForSeconds(.5f);
        action1 = true;
    } 

    public void ResumePlaying()
    {
        Time.timeScale = 1;
        gameManager.uiManager.HideTutorialMsg();
    }

    public void Action2()
    {
        gameManager.uiManager.ShowTutorialMsg("Release to shoot from afar!", 0);
        // Time.timeScale = 0;
        gameManager.enemiesManager.afterKillingAllSectionEnemiesEvents += ()=> {
            gameManager.uiManager.HideTutorialMsg();
            FirebaseAnalytics.LogEvent("Tutorial_Action2_Completed");
        };
        FirebaseAnalytics.LogEvent("Tutorial_Action2_Started");
    }

    // IEnumerator WaitAndSetAction2Click()
    // {
    //     yield return new WaitForSeconds(2);
    //     action2 = true;
    // }

    // add to all enemies killed action
    public void Action3()
    {
        gameManager.uiManager.infinityPanel.onClickEvents+= ()=> gameManager.uiManager.ShowTutorialMsg("Release to shoot from afar!", 0);
        // Time.timeScale = 0;
        gameManager.enemiesManager.afterKillingAllSectionEnemiesEvents += ()=> 
        {
            gameManager.uiManager.HideTutorialMsg();
            FirebaseAnalytics.LogEvent("Tutorial_Action3.1_Completed");
        };
        FirebaseAnalytics.LogEvent("Tutorial_Action3.1_Started");
        
        gameManager.uiManager.onClickReturnToMenuActions+= ()=> 
        {
            gameManager.uiManager.PlaceTutorialCircleAndClick(gameManager.uiManager.playButton.transform);
            FirebaseAnalytics.LogEvent("Tutorial_Action3.2_Started");
        };

        gameManager.uiManager.tutorialClickActions+= ()=> 
        {
            gameManager.uiManager.OnClickStartPanel();
            FirebaseAnalytics.LogEvent("Tutorial_Action3.2_Completed");
        };
    }

    public void Action4()
    {
        FirebaseAnalytics.LogEvent("Tutorial_Action4.1_Started");        

        gameManager.uiManager.onClickReturnToMenuActions+= ()=> 
        {
            PowerUpButton powerUpButton = PowerUpSelection.instance.GetPowerUpCollectionButton(0);
            gameManager.uiManager.PlaceTutorialCircleAndClick(powerUpButton.transform);

            gameManager.uiManager.tutorialClickActions += ()=> {
                 PowerUpSelection.instance.MoveFrom_Collection_To_Selection(powerUpButton);
                    Invoke("Action4SubAction", .1f);
                    FirebaseAnalytics.LogEvent("Tutorial_Action4.1_Completed");        
                };

            // gameManager.uiManager.tutorialClickActions += ()=> {
            //     Action4SubAction();
            // };
        };
    }

    void Action4SubAction()
    {
        FirebaseAnalytics.LogEvent("Tutorial_Action4.2_Started");        

        gameManager.uiManager.tutorialClickActions += ()=>
        {
            gameManager.uiManager.mm_Footer.SetButtonVisual(2);
            FirebaseAnalytics.LogEvent("Tutorial_Action4.2_Completed");        
        };
        gameManager.uiManager.PlaceTutorialCircleAndClick(gameManager.uiManager.mm_Footer.footerButtons[2].transform, new Vector3(20, 80, 0));
    }

    public void Action5()
    {
        FirebaseAnalytics.LogEvent("Tutorial_Action5_Started");        
        
        gameManager.uiManager.tutorialClickActions += ()=> PlayerManager.instance.powerUpsController.GetPowerUpButton(0).button.onClick.Invoke();

        gameManager.uiManager.infinityPanel.onClickEvents+= () => {
            gameManager.uiManager.ShowTutorialMsg("Use skill to launch a powerful attack");
            gameManager.uiManager.PlaceTutorialCircleAndClick(PlayerManager.instance.powerUpsController.GetPowerUpButton(0).transform);
            gameManager.enemiesManager.afterKillingAllSectionEnemiesEvents += ()=> 
            {
                FirebaseAnalytics.LogEvent("Tutorial_Action5_Completed");        
                gameManager.uiManager.HideTutorialMsg();
            };
        };
    }
    #endregion
}