using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    private UiManager uiManager;
    [SerializeField] private TMP_Text healthText; 
    
    void Start()
    {
        currentHealth = maxHealth;
        uiManager = FindObjectOfType<UiManager>();
        UpdateHealthText();
    }

    public void takeDamage()
    {
        currentHealth--;
        if (currentHealth < 0) 
        currentHealth = 0;

        UpdateHealthText();

        if(currentHealth == 0)
        {
            uiManager.onGameLoseShown();
        }
    }

    private void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString();
    }

    public void resetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }
}
