using UnityEngine;
using System.Collections.Generic;

public class PowerUpSelection : MonoBehaviour
{
    #region variables

    [SerializeField] PowerUpPurchase powerUpPurchase;

    [SerializeField] List<PowerupData> allPowerUps;

    // 3 3 slots of power-up buttons in power-up selection and home screen
    [SerializeField] PowerUpButton[] homeScreenButtons;
    [SerializeField] Transform[] selectionContainers;

    int powerupsInSelectionCount = 0;

    [SerializeField] PowerUpButton powerUpButtonPrefab;

    [SerializeField] Transform ToFindContainer, CollectedContainer;

    public Sprite coinSprite, gemSprite;

    //[HideInInspector]
    public List<PowerUpButton> toFindButtons, collectedButtons, selectionButtons;

    public bool removeFromSelectedMode;

    public int GetPowerupIndex(PowerupData data) => allPowerUps.IndexOf(data);
    public bool canAddToSelection => powerupsInSelectionCount < 3;
    public bool SelectionSlotsHasFreeOne => FindFreeSelectionSlot() >= 0;
    public PowerupData GetPowerupData(PowerUpType type) => allPowerUps[(int)type];

    public PowerUpButton GetPowerUpCollectionButton(int i) => collectedButtons[i];
    #endregion

    #region singleton

    public static PowerUpSelection instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    #region untiy
    bool started;
    private void OnEnable()
    {
        if (started)
            RefreshTofindButtons();
    }

    private void Start()
    {
        selectionButtons = new List<PowerUpButton>();
        toFindButtons = new List<PowerUpButton>();
        collectedButtons = new List<PowerUpButton>();
        PopulateToFindPowerUps();
        started = true;
    }
    #endregion

    #region refresh
    public void RefreshAllButtons()
    {
        RefreshTofindButtons();
        RefreshSelectionAndCollection();
    }

    public void RefreshSelectionAndCollection()
    {
        for (int i = 0; i < selectionButtons.Count; i++)
        {
            selectionButtons[i].RefreshUI();
        }

        for (int i = 0; i < collectedButtons.Count; i++)
        {
            collectedButtons[i].RefreshUI();
        }
    }
    #endregion

    #region selection-slots-playerprefs
    public void AddInCardValue(PowerUpType powerUpType, int val)
    {
        GetPowerupData(powerUpType).CurrentCards += val;
    }

    int SelectionSlotPref(int i)
    {
        return PlayerPrefs.GetInt("SelectionSlot" + i, -1);
    }
    void SetSelectionSlotPref(int i, int powerupDataIndex)
    {
        PlayerPrefs.SetInt("SelectionSlot" + i, powerupDataIndex);
    }
    int CheckIndexInSelectionSlots(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if(SelectionSlotPref(i) == index)
            {
                return i;
            }
        }
        return -1;
    }
    #endregion

    #region populate-buttons
    public void AddToSelection(PowerUpButton button)
    {
        //int freeIndex = 0;
        //for (int i = 0; i < powerupButtonsInSelection.Length; i++)
        //{
        //    if (!powerupButtonsInSelection[i].gameObject.activeSelf)
        //    {
        //        freeIndex = i;
        //        break;
        //    }
        //}

        //powerupsInSelectionCount++;
        //powerupButtonsInSelection[freeIndex].RefreshUI(data, PowerUpState.ShowingInSelction);
        // ------------------------------------------------------------------------------------
        if(canAddToSelection)
        {
            selectionButtons.Add(button);
            button.SetContainer(selectionContainers[powerupsInSelectionCount]);
            powerupsInSelectionCount++;
        }
    }

    void RefreshTofindButtons()
    {
        for (int i = 0; i < toFindButtons.Count; i++)
        {
            if (PlayerPrefsContainer.PlayerLevel >= toFindButtons[i].data.unlockOnLevel)
                MoveFrom_ToFind_To_Collection(toFindButtons[i]);
        }
    }

    void PopulateToFindPowerUps()
    {
        PowerUpButton newBtn;
        for (int i = 0; i < allPowerUps.Count; i++)
        {
            //print("index = " + i);
            // power-up is locked
            if (allPowerUps[i].CurrentLevel < 0)
            {
                //print("1");
                // player has achieved level to unlock this power-up level
                if (PlayerPrefsContainer.PlayerLevel >= allPowerUps[i].unlockOnLevel)
                {
                    //print("2");
                    allPowerUps[i].CurrentLevel = 0;
                    newBtn = Instantiate(powerUpButtonPrefab, CollectedContainer);
                    newBtn.RefreshUI(allPowerUps[i], PowerUpState.Collected);
                    collectedButtons.Add(newBtn);
                }
                // put power-up button in to-find section
                else
                {
                    //print("3");
                    newBtn = Instantiate(powerUpButtonPrefab, ToFindContainer);
                    newBtn.RefreshUI(allPowerUps[i], PowerUpState.ToFind);
                    toFindButtons.Add(newBtn);
                }
            }
            // put power-up button is UNLOCKED
            else
            {
                //print("4");
                // check if this button was in the selection last time
                int selectionSlotIndex = CheckIndexInSelectionSlots(i);
                if (selectionSlotIndex > -1)
                {
                    //print("5");
                    newBtn = Instantiate(powerUpButtonPrefab, selectionContainers[selectionSlotIndex]);
                    newBtn.RefreshUI(allPowerUps[i], PowerUpState.Selction);
                    selectionButtons.Add(newBtn);
                }
                else
                {
                    //print("6");
                    newBtn = Instantiate(powerUpButtonPrefab, CollectedContainer);
                    newBtn.RefreshUI(allPowerUps[i], PowerUpState.Collected);
                    collectedButtons.Add(newBtn);
                }
            }
        }
    }

    private void OnDisable()
    {
        PopulateHomeButtons();
        if (removeFromSelectedMode)
            ShakeSelectionButtons(null, false);
    }

    public void PopulateHomeButtons()
    {
        int j = 0;
        for (int i = 0; i < homeScreenButtons.Length; i++)
        {
            if (selectionContainers[i].childCount > 0)
                homeScreenButtons[i].RefreshUI(selectionButtons[j++].data, PowerUpState.HomeScreen);
            else
                homeScreenButtons[i].SetVisibility(false);
        }
    }
    #endregion

    #region test-purposes
    [ContextMenu("RefreshCollectedAndToFind")]
    public void RefreshCollectedAndToFind()
    {
        for (int i = 0; i < toFindButtons.Count; i++)
        {
            if(toFindButtons[i].data.CurrentLevel >= 0)
            {
                MoveFrom_ToFind_To_Collection(toFindButtons[i]);
            }
        }
    }

    [ContextMenu("LockAllPowerUps")]
    void LockAllPowerUps()
    {
        for (int i = 0; i < allPowerUps.Count; i++)
        {
            allPowerUps[i].CurrentLevel = -1;
        }
    }
    #endregion

    #region move-from-to
    void MoveFrom_ToFind_To_Collection(PowerUpButton button)
    {
        toFindButtons.Remove(button);
        collectedButtons.Add(button);

        button.SetContainer(CollectedContainer);
        if (button.data.CurrentLevel < 0)
            button.data.CurrentLevel = 0;
        button.RefreshUI(PowerUpState.Collected);
    }

    public void MoveFrom_Selection_To_Collection(PowerUpButton button)
    {
        selectionButtons.Remove(button);
        collectedButtons.Add(button);

        powerupsInSelectionCount--;

        SetSelectionSlotPref(GetButtonIndexInSelctionsSlots(button), -1);
        button.SetContainer(CollectedContainer);

        button.RefreshUI(PowerUpState.Collected);
    }

    // move from selction to collection
    public void MoveFrom_Collection_To_Selection(PowerUpButton button)
    {
        powerupsInSelectionCount++;

        collectedButtons.Remove(button);
        selectionButtons.Add(button);

        int freeSlotIndex = FindFreeSelectionSlot();
        button.indexInSelctionsSlots = freeSlotIndex;
        button.SetContainer(selectionContainers[freeSlotIndex]);

        SetSelectionSlotPref(freeSlotIndex, GetPowerupIndex(button.data));

        button.RefreshUI(PowerUpState.Selction);
    }

    public int FindFreeSelectionSlot()
    {
        for (int i = 0; i < selectionContainers.Length; i++)
        {
            if (selectionContainers[i].childCount == 0)
                return i;
        }

        return -1;
    }

    PowerUpButton buttonToPutInSelectionAfterRemoval;
    public void ShakeSelectionButtons(PowerUpButton button, bool state = true)
    {
        buttonToPutInSelectionAfterRemoval = button;
        removeFromSelectedMode = state;
        for (int i = 0; i < selectionButtons.Count; i++)
        {
            selectionButtons[i].Shake(state);
        }
    }

    int GetButtonIndexInSelctionsSlots(PowerUpButton button)
    {
        for (int i = 0; i < selectionContainers.Length; i++)
        {
            if (selectionContainers[i].childCount > 0)
                if (selectionContainers[i].GetChild(0).GetComponent<PowerUpButton>() == button)
                    return i;
        }
        return -1;
    }

    #endregion

    #region clicks
    public void OnClickPowerUpButton(PowerUpButton button)
    {
        if(button.state == PowerUpState.Selction)
        {
            if(removeFromSelectedMode)
            {
                removeFromSelectedMode = false;
                MoveFrom_Selection_To_Collection(button);
                MoveFrom_Collection_To_Selection(buttonToPutInSelectionAfterRemoval);
            }
            else
            {
                powerUpPurchase.Show(button);
            }
        }else if(button.state == PowerUpState.Collected)
        {
            powerUpPurchase.Show(button);
        }
    }
    #endregion
}