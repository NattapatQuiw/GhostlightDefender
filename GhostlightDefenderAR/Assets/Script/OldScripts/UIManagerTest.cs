using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManagerTest : MonoBehaviour
{
    [Header("Button Setup")] 
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button shootButton;
    
    [Header("UI Setup")]
    [SerializeField] private TMP_Text greetingText;
    [SerializeField] private TMP_Text scoreText;    //New code

    [Header("Image Setup")]                         //New code
    [SerializeField] private Image crosshair;           //New code
    
    public static event Action OnStartButtonPressed;
    public static event Action OnRestartButtonPressed;
    public static event Action OnShootButtonPressed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(OnUIStartButtonPressed);
        restartButton.onClick.AddListener(OnUIRestartButtonPressed);
        shootButton.onClick.AddListener(OnUIShootButtonPressed);
        
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        
        //New code week 11
        scoreText.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);
        scoreText.text = "SCORE: 0";
    }

    void OnUIStartButtonPressed()
    {
        OnStartButtonPressed?.Invoke();
        
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        shootButton.gameObject.SetActive(true);
        //greetingText.text = "SHOOT THE ENEMY!!!";
        greetingText.gameObject.SetActive(false);
        
        //New code week 11
        scoreText.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(true);
    }

    void OnUIRestartButtonPressed()
    {
        OnRestartButtonPressed?.Invoke();
        
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        greetingText.gameObject.SetActive(true);
        
        //New code week 11
        scoreText.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);
    }

    void OnUIShootButtonPressed()
    {
        OnShootButtonPressed?.Invoke();
    }

    //New code week 11
    public void UpdateDateScore(int score)
    {
        scoreText.text = $"SCORE: {score}";
    }

    
}
