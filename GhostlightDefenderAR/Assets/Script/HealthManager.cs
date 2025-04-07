using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    [SerializeField] private TMP_Text healthText; 
    
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void takeDamage()
    {
        currentHealth--;
        if (currentHealth < 0) 
        currentHealth = 0;

        UpdateHealthText();
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
