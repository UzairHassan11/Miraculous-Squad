using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PowerUpButton : MonoBehaviour
{
    public PowerUpState state;

    [SerializeField] Image iconImage, reloadFill, cardsProgressBar;
    [SerializeField] TextMeshProUGUI title, currentLevel, lockedMsg, cardsText, levelReward, coinsGemsReward;

    public Button button;

    public Sprite GetIconImageSprite => iconImage.sprite;

    public PowerUpLevel currentPowerUpLevel;
    public PowerupData data;
    public GameObject upgradeSection, upgradeIndicator, currentLevelSection, levelRewardSection;
    //[HideInInspector]
    public int indexInSelctionsSlots;
    [HideInInspector]
    public PowerUpEffect powerUpEffect;

    private void Start()
    {
        if (button)
            button.onClick.AddListener(OnClickButton);
    }

    public void RefreshUI(PowerUpButton button)
    {
        RefreshUI(button.data, button.state);
    }

    public void RefreshUI()
    {
        RefreshUI(data, state);
    }

    public void RefreshUI(PowerUpState _state)
    {
        RefreshUI(data, _state);
    }

    public void RefreshUI(PowerupData _data, PowerUpState _state)
    {
        state = _state;
        data = _data;
        if(!data.IsLocked)
            currentPowerUpLevel = _data.currentPowerUpLevel;
        bool maxedOut;

        if(levelRewardSection)
            levelRewardSection.SetActive(false);

        SetVisibility(true);
        switch (state)
        {
            case PowerUpState.ToFind:
                iconImage.sprite = _data.lockedIcon;
                lockedMsg.text = "Unlock at Level " + _data.unlockOnLevel;
                title.text = "";
                currentLevelSection.SetActive(false);
                upgradeSection.SetActive(false);
                break;

                // -------------------------------------------------------------------------------------

            case PowerUpState.Collected:
                ShowForCollectedAndSelected();
                break;
            case PowerUpState.Selction:
                ShowForCollectedAndSelected();
                break;

                // -------------------------------------------------------------------------------------

            case PowerUpState.HomeScreen:

                iconImage.sprite = _data.unlockedIcon;
                title.text = _data.title;

                maxedOut = _data.HasReachedMax;
                currentLevel.text = maxedOut ? "Max" : (_data.CurrentLevel + 1).ToString();

                upgradeSection.SetActive(false);
                break;
            case PowerUpState.Gameplay:

                iconImage.sprite = _data.unlockedIcon;
                title.text = data.title;

                currentLevelSection.SetActive(false);
                upgradeSection.SetActive(false);
                break;
            case PowerUpState.LevelReward:
                ShowForCollectedAndSelected();
                button.interactable = false;
                upgradeSection.SetActive(false);
                currentLevelSection.SetActive(false);
                break;
        }

        //name = _data.title + " - PowerUp-InGmae-Button";

        //iconImage.sprite = _data.unlockedIcon;
        //title.text = _data.title;


        //if(currentLevel)
        //    currentLevel.text = _data.HasReachedMax ? "Max" : (_data.CurrentLevel+1).ToString();
    }

    public void SetLevelRewardText(string str)
    {
        if(levelRewardSection)
        {
            levelRewardSection.SetActive(true);
            levelReward.text = str;
            coinsGemsReward.gameObject.SetActive(false);
        }
    }

    public void ShowLevelReward(int lvl, bool forCoin)
    {
        iconImage.sprite = forCoin ? GameManager.instance.uiManager.powerUpSelection.coinSprite : GameManager.instance.uiManager.powerUpSelection.gemSprite;
        title.text = "";
        SetVisibility(true);

        currentLevelSection.SetActive(false);
        lockedMsg.text = "";
        cardsText.text = "";
        if(levelRewardSection)
            levelRewardSection.SetActive(false);
        coinsGemsReward.gameObject.SetActive(true);
        coinsGemsReward.text = "+" + lvl;
        upgradeSection.SetActive(false);
        upgradeIndicator.SetActive(false);
    }

    void ShowForCollectedAndSelected()
    {
        iconImage.sprite = data.unlockedIcon;
        title.text = data.title;

        bool maxedOut;
        maxedOut = data.HasReachedMax;
        currentLevelSection.SetActive(true);
        currentLevel.text = maxedOut ? "Max" : (data.CurrentLevel + 1).ToString();
        if(maxedOut)
            cardsText.text = "";
        lockedMsg.text = "";

        if (!maxedOut)
        {
            if (upgradeSection)
                upgradeSection.SetActive(true);

            cardsText.text = data.CurrentCards + "/" + data.RequiredCards;
            cardsProgressBar.fillAmount = data.CurrentCards / (float)data.RequiredCards;
            if (data.HasRequiredCards && upgradeIndicator)
            {   if(PlayerPrefsContainer.PlayerCoins >= data.GetNextUpgradePrice)
                    {
                        upgradeIndicator.SetActive(true);
                        upgradeSection.SetActive(false);
                    }
            }
            else
            {
                upgradeIndicator.SetActive(false);
               upgradeSection.SetActive(true);
            }
            
            //if (!data.HasRequiredCards)
            //{
            //    //print(name + " - cards - " + data.CurrentCards + "/" + data.RequiredCards);
            //    cardsText.text = data.CurrentCards + "/" + data.RequiredCards;
            //    cardsProgressBar.fillAmount = data.CurrentCards / (float)data.RequiredCards;
            //}
            //else
            //{
            //    if(upgradeIndicator)
            //        upgradeIndicator.SetActive(PlayerPrefsContainer.PlayerCoins >= data.GetNextUpgradePrice);
            //}
        }
        else
        {
            if (upgradeSection)
                upgradeSection.SetActive(false);
            upgradeIndicator.SetActive(false);
        }
    }

    public void FillRloadImage(float fillTime)
    {
        reloadFill.fillAmount = 1;
        reloadFill.DOFillAmount(0, fillTime).SetEase(Ease.Linear);
    }

    public void OnClickButton()
    {
        PowerUpSelection.instance.OnClickPowerUpButton(this);
        // print(name + " say : came here");
    }

    public void SetContainer(Transform container)
    {
        transform.SetParent(container);
    }

    public void Shake(bool shake)
    {
        if (shake)
            transform.DOShakeRotation(.1f, new Vector3(0,0,5)).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        else
            DOTween.Kill(gameObject);
    }

    public void SetVisibility(bool state)
    {
        gameObject.SetActive(state);
    }

    public bool isVisible => gameObject.activeSelf;
}
public enum PowerUpState
{
    ToFind,
    Collected,
    Selction,
    HomeScreen,
    Gameplay,
    LevelReward
}