using System.Collections.Generic;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{
    public bool shieldMode;
    [SerializeField] GameObject shieldObject, fasterFire;
    [Tooltip("Just for the order of powerup scriptable objects and buttons")]
    [SerializeField]
    PowerUpType powerUpTypes;
    public PowerupData[] powerupDatas;
    bool[] powerUpReloadings;

    [SerializeField] PowerUpEffect[] powerUpEffects;
    public PowerUpButton[] powerUpButtons;

    public PowerUpButton GetPowerUpButton(int i) => powerUpButtons[i];

    // Start is called before the first frame update
    void Start()
    {
        powerUpReloadings = new bool[powerupDatas.Length];
    }

    public void Init()
    {
        PopulateButtons();
    }

    void PopulateButtons()
    {
        List<PowerUpButton> selectionButtons = PowerUpSelection.instance.selectionButtons;
        PowerUpEffect powerUpEffect;
        for (int i = 0; i < powerUpButtons.Length; i++)
        {
            //print("Index = " + i);
            if (i < selectionButtons.Count)
            {
                //print("1");
                powerUpButtons[i].RefreshUI(selectionButtons[i].data, PowerUpState.Gameplay);
                powerUpButtons[i].SetVisibility(true);
                powerUpEffect = powerUpEffects[PowerUpSelection.instance.GetPowerupIndex(selectionButtons[i].data)];
                powerUpEffect.Init(powerUpButtons[i]);
                powerUpButtons[i].powerUpEffect = powerUpEffect;
            }
            else
            {
                //print("2");
                powerUpButtons[i].powerUpEffect = null;
                powerUpButtons[i].SetVisibility(false);
            }
        }
    }

    void ResetAll()
    {
        List<PowerUpButton> selectionButtons = PowerUpSelection.instance.selectionButtons;
        for (int i = 0; i < powerUpButtons.Length; i++)
        {
            //print("Index = " + i);
            if (powerUpButtons[i].isVisible)
            {
                powerUpButtons[i].powerUpEffect.ResetAll();
            }
        }
    }

    public void ResetInGamePowerUps()
    {
        for (int i = 0; i < powerUpButtons.Length; i++)
        {
            if (powerUpButtons[i].isVisible)
                powerUpButtons[i].RefreshUI();
        }
    }

    [SerializeField] SlashEffect slashEffect;
    public void SlashEnemy(Enemy enemy)
    {
        slashEffect.AttackWithSlash(enemy);
    }

    public float GetCurrentValue(int index)
    {
        return powerupDatas[index].GetCurrentValue;
    }

}