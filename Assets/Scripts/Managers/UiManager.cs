using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class UiManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText, diamondsText, playerLevelText,
        winScreenLootCoins, winScreenLootGems, tutorialMsgText,
        winScreenRewardCoins, winScreenRewardGems, bossName;

    [SerializeField] Button reward2xButton, reviveButton;

    [SerializeField] private Text levelText;
    
    [SerializeField] private bool test;
    
    [SerializeField] private GameObject startPanel, gameplayPanel, winPanel, failPanel, fadePanel,
        header, continueButton, openButton, levelRewardPanel, levelCollectedRewardsPanel,
        winScreenRewardsPacket, winScreenCoinsPacket, winScreenGemsPacket, winScreenLootCoinsContainer,
        winScreenLootGemsContainer, settingPanelInGame, tutorialMsgSection, tutorialPanel, tutorialCircleAndHand, tutorialMsgImage1;

    [SerializeField] GameObject [] tutorialMsgImages;
    [SerializeField] Transform levelSectionInidcatorsContainer;

    LevelSelection levelSelection;

    [SerializeField] Image playerLevelFill;

    [SerializeField] PowerUpButton[] levelRewardButtons;

    public PowerUpSelection powerUpSelection;
    public MM_Footer mm_Footer;

    public GameObject playButton;
    [SerializeField] HealthBar bossHealthBar;

    bool bossLevel;

    int gemsLooted, coinsLooted;

    public InfinityPanel infinityPanel;

    [SerializeField] Image slashClaw;

    // Start is called before the first frame update
    void Start()
    {
        //levelText.text = "Level " + LevelNumberAnalytics.ToString("00");
        UpdateCoinsText();
        UpdateGemsText();
        UpdatePlayerLevel();
        //if (!test)
        //{
            // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LevelStart", LevelNumberAnalytics);
        //}
        levelSelection = GameManager.instance.levelSelection;
    }

    public UnityAction tutorialClickActions, onClickReturnToMenuActions;
    Transform tutorialTarget;
    Vector3 currentOffset;
    public void PlaceTutorialCircleAndClick(Transform t, Vector3 offset = default)
    {
        tutorialCircleAndHand.SetActive(true);
        tutorialPanel.SetActive(true);
        currentOffset = offset;
        // tutorialCircleAndHand.transform.SetParent(t);
        // tutorialCircleAndHand.GetComponent<RectTransform>().localPosition = Vector3.zero + offset;
        // tutorialCircleAndHand.transform.DOMove(t.position, 0).SetDelay(.1f);
        tutorialTarget = t;
        // tutorialCircleAndHand.transform.SetParent(tutorialPanel.transform);
    }
    void Update()
    {
        if(tutorialPanel.activeSelf)
        {
            tutorialCircleAndHand.transform.position = tutorialTarget.position + currentOffset;
        }
    }

    public void OnClickRetry()
    {
        // assign actions to RV call back to the following function
        AdController.Instance.ShowRewardedAd((isRewarded) =>
        {
            if(isRewarded)
            {
                RetryLevel();

                rvRewarded = true;
                AdController.Instance.RestartInterstitialTimer();
            }
        });
    }

    public void RetryLevel()
    {
        PlayerManager.instance.Revive();
        failPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        // rvRewarded = false;
        GameManager.instance.ChangeGameState(GameState.Running);
        SoundManager.Instance.PlayMainMenyBG(false);
        GameManager.instance.enemiesManager.UntriggerAllEnemies();
        ShowGems(false);
        ShowCoin(false);
    }

    public void HandleTutorialClick()
    {
        tutorialPanel.SetActive(false);
        // tutorialCircleAndHand.transform.SetParent(tutorialPanel.transform);
        tutorialCircleAndHand.SetActive(false);
        // if(tutorialClickActions==null)
        //     Debug.LogError("tutorialClickActions = null");
        // else
        //     Debug.LogError("tutorialClickActions != null");

        tutorialClickActions?.Invoke();
        tutorialClickActions = null;
    }

    public void ShowTutorialMsg(string str, int tutorialMsgImageIndex = -1)
    {
        tutorialMsgText.text = str;
        tutorialMsgSection.SetActive(true);
        if(tutorialMsgImageIndex > -1)
            tutorialMsgImages[tutorialMsgImageIndex].SetActive(true);
    }

    public void OnGotItTutPanel()
    {
        TutorialController.instance.ResumePlaying();
    }

    public void HideTutorialMsg()
    {
        tutorialMsgSection.SetActive(false);
    }

    public void ShowGameplayPanel(bool state)
    {
        gameplayPanel.SetActive(state);
        if(state)
           rvRewarded = false;
    }

    bool rvRewarded;
    public void OnClick2xLoot()
    {
        // check if ad is avaialble
        // if(true)
        // {
        //     // set to RV reward call-back
        //     Reward2xLoot();
        // }

        AdController.Instance.ShowRewardedAd((isRewarded) =>
        {
            if(isRewarded)
               {
                 Reward2xLoot();
                rvRewarded = true;
               AdController.Instance.RestartInterstitialTimer();
               }
        });
    }
    public void Reward2xLoot()
    {
        gemsLooted *= 2;
        coinsLooted *= 2;
        SetLootsTexts(true);
        reward2xButton.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound(ClipName.Reward);
    }

    public void OpenSettingPanelInGame()
    {
        settingPanelInGame.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnResumeClicked()
    {
        settingPanelInGame.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        //if (levelSelection.CurrentLevelIndex == 0)
        //    SceneManager.LoadScene(0);
        GameManager.instance.ChangeGameState(GameState.Idle);

        if (settingPanelInGame.activeSelf)
        {
            settingPanelInGame.SetActive(false);
            Time.timeScale = 1;
        }

        onClickReturnToMenuActions?.Invoke();
        onClickReturnToMenuActions = null;

        SoundManager.Instance.PlayMainMenyBG(true);
        GameManager.instance.levelManager.TurnOffLastPlatform();
        GameManager.instance.enemiesManager.EmptyEnemiesList();
        PoolManager.instance.TurnOffAllObjects(PoolableObjectType.Coins);
        PlayerManager.instance.ResetAll();
        //PlayerManager.instance.playerController.ResetFirstTimeFall();

        startPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        winPanel.SetActive(false);
        failPanel.SetActive(false);

        ShowCoin(true);
        ShowGems(true);
        ShowPlayerLevel(true);

        powerUpSelection.RefreshAllButtons();

        if(!rvRewarded)
           if(levelSelection.CurrentEnvironmentIndex > 0)
                AdController.Instance.CheckRemoteTimeAndShowInterstitialAd();

        if(levelSelection.CurrentEnvironmentIndex > 0)
            AdController.Instance.HideBanner();
    }

    public void SetRvRewarded(bool state)
    {
        rvRewarded = state;
    }

    public void OnClickStartPanel()
    {
        startPanel.SetActive(false);
        //gameplayPanel.SetActive(true);

        GameManager.instance.ChangeGameState(GameState.Running);

        ShowLevelSectionInidcators();

        ShowCoin(false);
        ShowGems(false);
        ShowPlayerLevel(false);

        if (!test)
        {
            CameraManager.instance.SetAnimatorState(CamStates.gameplay);
            GameManager.instance.ChangeGameState(GameState.Running);
        }

        if(levelSelection.CurrentEnvironmentIndex > 0)
            AdController.Instance.ShowBanner();
    }
    void ShowLevelSectionInidcators()
    {
        if (levelSelection.isLastLevel)
            return;
        int curSectCount = levelSelection.CurrentLevelSectionsCount;
        for (int i = 0; i < levelSectionInidcatorsContainer.childCount; i++)
        {
            levelSectionInidcatorsContainer.GetChild(i).gameObject.SetActive(i < curSectCount);
        }
    }

    public void TurnLevelSectionInidcatorsContainer(bool state)
    {
        levelSectionInidcatorsContainer.gameObject.SetActive(state);
    }

    public void ShowWinPanel(float delay = 0)
    {
        if(SaveData.Instance.Haptic)
            MMVibrationManager.Haptic(HapticTypes.Success);
        //storing level rewards here because current-level increases after this
        levelRewards = GameManager.instance.levelSelection.CurrentLevel.levelReward;
        int levelUpAmount = GameManager.instance.levelSelection.CurrentLevel.levelUpAmount;
        SoundManager.Instance.TurnOffBgSound();
        SoundManager.Instance.PlaySound(ClipName.Win);

        if (levelUpAmount > 0)
        {
            PlayerPrefsContainer.PlayerLevel += levelUpAmount;
            UpdatePlayerLevel();
            ShowPlayerLevel(true);
        }

        if (delay > 0)
        {
            StartCoroutine(showPanelWithDelay(delay, true));
            return;
        }
        
        if (!test)
        {
            // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level_Completed", LevelNumberAnalytics);
        }

        // 2x button
        // check if rewarded ad is available
        if(GameManager.instance.levelSelection.CurrentEnvironmentIndex == 0)
            reward2xButton.gameObject.SetActive(false);
        else
        {  
            reward2xButton.gameObject.SetActive(true);

            if (AdController.Instance.IsRewardedAdAvailable())
            {
                reward2xButton.interactable = true;
            }
            else
            {
                reward2xButton.interactable = false;
            }
        }

        TurnLevelSectionInidcatorsContainer(false);

        winPanel.SetActive(true);
        gameplayPanel.SetActive(false);

        continueButton.SetActive(false);
        openButton.SetActive(false);
        winScreenRewardsPacket.SetActive(false);
        winScreenCoinsPacket.SetActive(false);
        winScreenGemsPacket.SetActive(false);
        winScreenRewardsPacket.SetActive(false);

        coinsLooted = PlayerManager.instance.coinsCollector.coinsCollected;
        gemsLooted = PlayerManager.instance.coinsCollector.gemsCollected;

        SetLootsTexts();

        if (GameManager.instance.levelSelection.hasPacketReward)
        {
            continueButton.SetActive(false);
            openButton.SetActive(true);
            winScreenRewardsPacket.SetActive(true);
        }
        else
        {
            continueButton.SetActive(true);
            openButton.SetActive(false);
            winScreenRewardsPacket.SetActive(false);
            if (GameManager.instance.levelSelection.hasCoinsReward)
            {
                winScreenCoinsPacket.SetActive(true);
                winScreenRewardCoins.text = "+" + levelSelection.GetRewardAmount(0).ToString();
            }
            else if (GameManager.instance.levelSelection.hasGemsReward)
            {
                winScreenGemsPacket.SetActive(true);
                winScreenRewardGems.text = "+" + levelSelection.GetRewardAmount(0).ToString();
            }
        }

        if (bossLevel)
        {
            bossLevel = false;
        }

        ShowGems(true);
        ShowCoin(true);

        //LevelNumberAnalytics++;
        //LevelNumberPref++;
        //if (LevelNumberPref >= SceneManager.sceneCountInBuildSettings)
        //    LevelNumberPref = 1;
    }
    void SetLootsTexts(bool animate = false)
    {
        if (coinsLooted > 0)
        {
            winScreenLootCoinsContainer.SetActive(true);
            winScreenLootCoins.text = coinsLooted.ToString();
            if (animate)
                winScreenLootCoinsContainer.transform.DOScale(.2f, .2f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else
            winScreenLootCoinsContainer.SetActive(false);

        if (gemsLooted > 0)
        {
            winScreenLootGemsContainer.SetActive(true);
            winScreenLootGems.text = gemsLooted.ToString();
            if (animate)
                winScreenLootGemsContainer.transform.DOScale(.2f, .2f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else
            winScreenLootGemsContainer.SetActive(false);
    }

    public void ShowFailPanel(float delay = 0)
    {
        if(SaveData.Instance.Haptic)
            MMVibrationManager.Haptic(HapticTypes.Failure);

        if (delay > 0)
        {
            StartCoroutine(showPanelWithDelay(delay, false));
            return;
        }
        
        if (!test)
        {
            // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level_Failed", LevelNumberAnalytics);
        }

        SoundManager.Instance.TurnOffBgSound();
        SoundManager.Instance.PlaySound(ClipName.Fail);

        TurnLevelSectionInidcatorsContainer(false);

        failPanel.SetActive(true);

        if(levelSelection.CurrentEnvironmentIndex > 0)
        {
            reviveButton.gameObject.SetActive(true);
            reviveButton.interactable = AdController.Instance.IsRewardedAdAvailable();
        }
        else
            reviveButton.gameObject.SetActive(false);

        gameplayPanel.SetActive(false);

        ShowGems(true);

        bossLevel = false;
    }

    IEnumerator showPanelWithDelay(float delay, bool win)
    {
        yield return new WaitForSeconds(delay);
        if (win) 
            ShowWinPanel(0);
        else
            ShowFailPanel(0);
    }

    public void OnReloadLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void IndicateSection(int index)
    {
        if(index > 0)
            levelSectionInidcatorsContainer.GetChild(index-1).GetChild(0).gameObject.SetActive(false);

        levelSectionInidcatorsContainer.GetChild(index).GetChild(0).gameObject.SetActive(true);
    }

    public void ShowFadePanel()
    {
        fadePanel.SetActive(true);
    }

    public void UpdateCoinsText()
    {
        coinsText.text = PlayerPrefsContainer.PlayerCoins.ToString();
    }

    public void UpdatePlayerLevel()
    {
        playerLevelText.text = PlayerPrefsContainer.PlayerLevel.ToString();
        //playerLevelFill
    }

    public void UpdateGemsText()
    {
        diamondsText.text = PlayerPrefsContainer.PlayerGems.ToString();
    }

    public void HideHaader(bool state)
    {
        header.SetActive(state);
    }

    #region level-reward

    public void ShowLevelRewardPanel(bool state)
    {
        levelRewardPanel.SetActive(state);
    }

    int curLvlRwrd = 0;
    void HideAllLevelRewardButtons()
    {
        curLvlRwrd = 0;
        for (int i = 0; i < levelRewardButtons.Length; i++)
        {
            levelRewardButtons[i].SetVisibility(false);
        }
    }

    LevelReward[] levelRewards;
    public void ShowLevelRewards()
    {
        //print("0");
        LevelReward levelReward;
        curLvlRwrd = 0;
        for (int i = 0; i < levelRewards.Length; i++)
        {
            //print("in-loop");
            levelReward = levelRewards[i];

            if (levelReward.levelRewardType == LevelRewardType.PowerUp)
            {
                //print(i + " " + levelReward.levelRewardType);
                levelRewardButtons[curLvlRwrd].RefreshUI(powerUpSelection.GetPowerupData(levelReward.powerUpType), PowerUpState.LevelReward);
                levelRewardButtons[curLvlRwrd].SetLevelRewardText("x" + levelReward.rewardAmount);
            }
            else if (levelReward.levelRewardType == LevelRewardType.Coins)
            {
                //print(i + " " + levelReward.levelRewardType);
                levelRewardButtons[curLvlRwrd].ShowLevelReward(levelReward.rewardAmount, true);
            }
            else if (levelReward.levelRewardType == LevelRewardType.Gems)
            {
                //print(i + " " + levelReward.levelRewardType);
                levelRewardButtons[curLvlRwrd].ShowLevelReward(levelReward.rewardAmount, false);
            }
            curLvlRwrd++;
        }
        levelCollectedRewardsPanel.SetActive(true);
        ShowLevelRewardPanel(false);
    }

    public void OnClickCollectAllReward()
    {
        for (int i = 0; i < levelRewards.Length; i++)
        {
            if(levelRewards[i].levelRewardType == LevelRewardType.Coins)
            {
                PlayerPrefsContainer.PlayerCoins += levelRewards[i].rewardAmount;
                UpdateCoinsText();
            }else if (levelRewards[i].levelRewardType == LevelRewardType.Gems)
            {
                PlayerPrefsContainer.PlayerGems += levelRewards[i].rewardAmount;
                UpdateGemsText();
            }else if (levelRewards[i].levelRewardType == LevelRewardType.PowerUp)
            {
                powerUpSelection.AddInCardValue(levelRewards[i].powerUpType, levelRewards[i].rewardAmount);
            }
        }
        powerUpSelection.RefreshAllButtons();
        mm_Footer.SetButtonVisual(1);
        levelCollectedRewardsPanel.SetActive(false);
        HideAllLevelRewardButtons();
        ReturnToMainMenu();
    }
    #endregion

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("https://studionebula.io/privacy-policy");
    }

    [SerializeField] CanvasGroup coinsStuff, gemsStuff, playerLevelStuff;
    Tween showCoinTemp;
    public void ShowCoin(bool state, bool temp = false)
    {
        if (temp)
        {
            coinsStuff.alpha = 1;
            if (showCoinTemp != null)
                if (showCoinTemp.IsPlaying())
                    showCoinTemp.Kill();

            showCoinTemp = coinsStuff.DOFade(0, .5f).SetDelay(1f);

        }
        else
            coinsStuff.DOFade(state ? 1 : 0, 1);
        //coinsStuff.interactable = state;
        //coinsStuff.blocksRaycasts = !state;
    }

    public void ShowGems(bool state)
    {
        gemsStuff.DOFade(state ? 1 : 0, 1);
        //gemsStuff.interactable = state;
        //gemsStuff.blocksRaycasts = !state;
    }

    public void ShowPlayerLevel(bool state)
    {
        playerLevelStuff.DOFade(state ? 1 : 0, 1);
        //playerLevelStuff.interactable = state;
        //playerLevelStuff.blocksRaycasts = !state;
    }

    public void InitForBoss(Enemy boss, string _name)
    {
        bossLevel = true;
        boss.healthController.healthBar = bossHealthBar;
        bossName.text = _name;
    }

    public void PlaceSlashClaw(Transform t)
    {
        slashClaw.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(t.position);
        slashClaw.gameObject.SetActive(true);
    }
}