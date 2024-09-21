using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

public class PowerUpPurchase : MonoBehaviour
{
    [SerializeField] GameObject upgradedPanel;
    [SerializeField] Image upgradedIcon;
    [SerializeField] PowerUpButton buttonAttachtoMenu;
    PowerUpButton buttonBeingShown;

    [SerializeField] Text upgradedPanelText;
    [SerializeField] TextMeshProUGUI descriptionText, valueName, currentValue, nextValue, coinsToUnlockText;
    [SerializeField] Button upgradeButton, select, remove, close;

    private void Start()
    {
        close.onClick.AddListener(()=>SetVisibility(false));
        select.onClick.AddListener(OnSelectClicked);
        remove.onClick.AddListener(OnRemoveClicked);
    }

    public void Show(PowerUpButton _buttonBeingShown)
    {
        SetVisibility(true);

        buttonBeingShown = _buttonBeingShown;

        buttonAttachtoMenu.state = _buttonBeingShown.state;
        buttonAttachtoMenu.data = _buttonBeingShown.data;

        buttonAttachtoMenu.RefreshUI();

        descriptionText.text = buttonAttachtoMenu.data.description;
        valueName.text = buttonAttachtoMenu.data.valueName;


        nextValue.gameObject.SetActive(false);
        upgradeButton.interactable = false;

        remove.gameObject.SetActive(buttonAttachtoMenu.state == PowerUpState.Selction);
        select.gameObject.SetActive(buttonAttachtoMenu.state == PowerUpState.Collected);

        if (buttonAttachtoMenu.data.HasReachedMax)
        {
            currentValue.text = "Max";
            coinsToUnlockText.text = "0";
        }
        else
        {
            currentValue.text = buttonAttachtoMenu.data.valuePrefix + buttonAttachtoMenu.data.GetCurrentValue + buttonAttachtoMenu.data.valuePostfix;

            coinsToUnlockText.text = buttonAttachtoMenu.data.GetNextUpgradePrice.ToString();

            if (buttonAttachtoMenu.data.HasRequiredCards && PlayerPrefsContainer.PlayerCoins >= buttonAttachtoMenu.data.GetNextUpgradePrice)
            {
                nextValue.gameObject.SetActive(true);
                nextValue.text = buttonAttachtoMenu.data.valuePrefix + buttonAttachtoMenu.data.GetNextValue + buttonAttachtoMenu.data.valuePostfix;
                upgradeButton.interactable = true;
                upgradeButton.onClick.RemoveAllListeners();
                upgradeButton.onClick.AddListener(OnClickUpgradeButton);
            }
        }
    }
// void Update()
// {
//     if(buttonBeingShown)
//     print(buttonBeingShown.data.CurrentLevel);
// }
    void OnClickUpgradeButton()
    {
        PlayerPrefsContainer.PlayerCoins -= buttonAttachtoMenu.data.GetNextUpgradePrice;
        buttonBeingShown.data.CurrentCards -= buttonBeingShown.data.RequiredCards;
        buttonBeingShown.data.CurrentLevel++;
        // print("OnClickUpgradeButton");
        // SetVisibility(false);
        SoundManager.Instance.PlaySound(ClipName.Reward);
        Show(buttonBeingShown);
        buttonAttachtoMenu.RefreshUI();
        buttonBeingShown.RefreshUI();
        upgradedIcon.sprite = buttonBeingShown.GetIconImageSprite;
        upgradedPanel.SetActive(true);
        upgradedPanelText.text = buttonBeingShown.data.title;
        GameManager.instance.uiManager.UpdateCoinsText();
    }

    public void OnRemoveClicked()
    {
        //print("remove clicked");

        PowerUpSelection.instance.MoveFrom_Selection_To_Collection(buttonBeingShown);

        SetVisibility(false);
    }

    public void OnSelectClicked()
    {
        //print("1");
        if (PowerUpSelection.instance.SelectionSlotsHasFreeOne)
        {
            //print("2");
            PowerUpSelection.instance.MoveFrom_Collection_To_Selection(buttonBeingShown);
            SetVisibility(false);
        }
        else
        {
            //print("3");
            PowerUpSelection.instance.ShakeSelectionButtons(buttonBeingShown);
        }
    }

    public void SetVisibility(bool state)
    {
        gameObject.SetActive(state);
    }    
}