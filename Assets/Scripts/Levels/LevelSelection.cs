using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Firebase.Analytics;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] Text onBoarding;

    [SerializeField] Image iconImage;

    [SerializeField] CityOfLevels[] citiesOfLevels;

    [SerializeField] LevelIndicatorUI[] levelInidcators;

    [SerializeField] GameObject currentLevelIndicator;

    public int CurrentLevelIndex => PlayerPrefsContainer.CurrentLevel;

    public int CurrentEnvironmentIndex => PlayerPrefsContainer.CurrentEnvironment;

    public int CurrentLevelSectionsCount => citiesOfLevels[CurrentEnvironmentIndex].levels[CurrentLevelIndex].levelSections.Length;

    public bool isLastLevel => CurrentLevelIndex == citiesOfLevels[CurrentEnvironmentIndex].levels.Length - 1;
    public bool isLastEnv => CurrentEnvironmentIndex == citiesOfLevels.Length - 1;

    public int CurrentSection = 0;

    public Level CurrentLevel => citiesOfLevels[CurrentEnvironmentIndex].levels[CurrentLevelIndex];

    public Sprite coinRewardSprite, gemRewardSprite, packetRewardSprite;
    public bool hasPacketReward, hasCoinsReward, hasGemsReward;

    public CityOfLevels CurrentEnvironment => citiesOfLevels[CurrentEnvironmentIndex];

    // Start is called before the first frame update
    void Start()
    {
        UpdateIU();
        print("CurrentEnvironment " + CurrentEnvironmentIndex);
        print("CurrentLevel " + CurrentLevelIndex);
        //print(CurrentEnvironment);
    }

    public void UpdateIU()
    {
        hasPacketReward = false;
        hasCoinsReward = false;
        hasGemsReward = false;

        SetIndicatorUI();

        currentLevelIndicator.transform.SetParent(levelInidcators[CurrentLevelIndex > 0 ? CurrentLevelIndex - 1 : 0].destination.transform);
        currentLevelIndicator.transform.localPosition = Vector3.zero;

        //if(citiesOfLevels[PlayerPrefsContainer.CurrentEnvironment].levels[CurrentLevel].levelReward != null)
        //{
        //    levelInidcators[CurrentLevel].levelReward.gameObject.SetActive(true);
        //    levelInidcators[CurrentLevel].levelReward.sprite =
        //        citiesOfLevels[PlayerPrefsContainer.CurrentEnvironment].levels[CurrentLevel].levelReward.rewardSprite;
        //}

        if (onBoarding)
        {
            string str = PlayerPrefsContainer.CurrentEnvironment + ". " + citiesOfLevels[PlayerPrefsContainer.CurrentEnvironment].onBoarding;
            onBoarding.DOText(str, .5f, true, ScrambleMode.All).SetDelay(.25f);
        }
        if (iconImage)
            iconImage.sprite = citiesOfLevels[PlayerPrefsContainer.CurrentEnvironment].icon;

        //if (CurrentLevelIndex == 0)
        //    return;

        for (int i = 0; i < levelInidcators.Length; i++)
        {
            //print("-1");
            if (!levelInidcators[i].gameObject.activeSelf)
                break;

            //print("0");
            if (i < CurrentLevelIndex)
            {
                //print("1");
                if (levelInidcators[i])
                {
                    levelInidcators[i].FillImage();
                }
                if (levelInidcators[i].destination)
                    levelInidcators[i].destination.color = Color.yellow;
            }
            else
            {
                //print("2");
                if (levelInidcators[i])
                {
                    levelInidcators[i].UnfillImage();
                }
                if (levelInidcators[i].destination)
                    levelInidcators[i].destination.color = Color.white;
            }
        }
        if(levelInidcators[CurrentLevelIndex])
            levelInidcators[CurrentLevelIndex].FillImage(true);

        currentLevelIndicator.transform.SetParent(levelInidcators[PlayerPrefsContainer.CurrentLevel].destination.transform);
        currentLevelIndicator.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.Linear);
    }

    void SetIndicatorUI()
    {
        for (int i = 0; i < levelInidcators.Length; i++)
        {
            levelInidcators[i].gameObject.SetActive(i < citiesOfLevels[CurrentEnvironmentIndex].levels.Length);
        }

        LevelReward[] levelRewards;
        for (int i = 0; i < citiesOfLevels[CurrentEnvironmentIndex].levels.Length; i++)
        {
            levelInidcators[i].destination.color = Color.white;

            //{
            levelRewards = citiesOfLevels[CurrentEnvironmentIndex].levels[i].levelReward;
            //print("0");
            switch (levelRewards.Length)
            {
                case 0:
                    // turn of level-reward-sprite for levels having no reward
                    levelInidcators[i].rewardImage.gameObject.SetActive(false);
                    //print("1");
                    break;
                case 1:
                    levelInidcators[i].rewardImage.gameObject.SetActive(true);
                    levelInidcators[i].rewardImage.sprite = levelRewards[0].levelRewardType == LevelRewardType.Coins ? coinRewardSprite : gemRewardSprite;
//                    print("2");
                    if (CurrentLevelIndex == i)
                    {
                        hasCoinsReward = levelRewards[0].levelRewardType == LevelRewardType.Coins;
                        hasGemsReward = !hasCoinsReward;

                        //print("hasGemsReward " + hasGemsReward + "    " + i);
                    }
                    break;
                default:
                    levelInidcators[i].rewardImage.gameObject.SetActive(true);
                    levelInidcators[i].rewardImage.sprite = packetRewardSprite;
                    //print("3");
                    if (CurrentLevelIndex == i)
                    {
                        hasPacketReward = true;
                        //print("hasPacketReward " + i);
                    }
                    break;
                    //}
                    //}
                    //else
                    //{
                    //    // turn of level-reward-sprite for already levels
                    //    levelInidcators[i].rewardImage.gameObject.SetActive(false);
                    //    print("4");
                    //}
            }
        }
    }

    public int GetRewardAmount(int index) => CurrentLevel.levelReward[index].rewardAmount;

    public void IncreaseLevel()
    {
        //print("Increase Level");
        CurrentSection = 0;

        if (isLastLevel)
        {
            PlayerPrefsContainer.CurrentLevel = 0;
            if (isLastEnv)
                PlayerPrefsContainer.CurrentEnvironment = 1;
            else
                PlayerPrefsContainer.CurrentEnvironment++;

            FirebaseAnalytics.LogEvent("Cleared_Chapter_"+PlayerPrefsContainer.CurrentEnvironment);
        }
        else
            PlayerPrefsContainer.CurrentLevel++;
    }

    [ContextMenu("SectionCompleted")]
    public void SectionCompleted()
    {
        if(CurrentSection == citiesOfLevels[CurrentEnvironmentIndex].levels[CurrentLevelIndex].levelSections.Length - 1)
        {
            //level win
            //GameManager.instance.ChangeGameState(GameState.Win);
        }else
        {
            CurrentSection++;
            GameManager.instance.uiManager.IndicateSection(CurrentSection);
        }
    }
}