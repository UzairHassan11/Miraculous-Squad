using UnityEngine;
using Sirenix.OdinInspector;

public class HealthController : MonoBehaviour
{
    [SerializeField] float totalHealth;
    [ReadOnly]
    [SerializeField] float currentHealth;

    public HealthBar healthBar;

    float fillAmount;

    bool healthBarIsShowing;

    private void Start()
    {
        currentHealth = totalHealth;
        ShowHealthBar(false);
        healthBarIsShowing = false;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(!healthBarIsShowing)
        {
            healthBarIsShowing = true;
            ShowHealthBar(true);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            ShowHealthBar(false);
        }

        fillAmount = currentHealth / totalHealth;

        if(healthBar)
            healthBar.UpdateSlider(fillAmount);
    }

    public bool OutOfHeath => currentHealth <= 0;

    public void ShowHealthBar(bool state)
    {
        if(healthBar)
            healthBar.ShowHealthBar(state);
        
        healthBarIsShowing = state;
    }

    public void ResetHealth()
    {
        currentHealth = totalHealth;
        ShowHealthBar(false);
        if (healthBar)
            healthBar.UpdateSlider(1);
    }

    public void ResetHealth(float _totalHealth)
    {
        totalHealth = _totalHealth;
        ResetHealth();
    }

    public void SetHealthManually(int n)
    {
        currentHealth = n;
    }

    public void SetHealthToMax()
    {
        currentHealth = totalHealth;
    }
}