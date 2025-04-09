using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class UiManager : MonoBehaviour
{
    [Header("Button Setup")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button retryButton;

    [Header("UI Setup")]
    [SerializeField] private GameObject scanText;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject timeCountUi;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject winLoseUi;
    [SerializeField] private GameObject winUiShown;
    [SerializeField] private GameObject loseUiShown;

    [Header("Pausegame Setup")]
    [SerializeField]private bool isPause = false;
    [SerializeField] private TimeCountDown timeCount;
    
    public static event Action OnStartButtonPressed;
    public static event Action OnPauseButtonPressed;
    public static event Action OnResumeButtonPressed;
    public static event Action OnRetryButtonPressed;
    private void Start()
    {
        startButton.onClick.AddListener(OnUIStartButtonPressed);
        pauseButton.onClick.AddListener(OnUIPauseButtonPressed);
        resumeButton.onClick.AddListener(OnUIResumeButtonPressed);
        retryButton.onClick.AddListener(OnUIRetryButtonPressed);

        heart.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        timeCountUi.gameObject.SetActive(false);
        flashlight.gameObject.SetActive(false);
        winLoseUi.gameObject.SetActive(false);
    }
    private void OnUIStartButtonPressed()
    {
        OnStartButtonPressed?.Invoke();
        scanText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        heart.gameObject.SetActive(true);
        timeCountUi.gameObject.SetActive(true);
        flashlight.gameObject.SetActive(true);
        timeCount.StartCountdown();
        SoundManager.Instance.PlayClickSound();
    }
    private void OnUIPauseButtonPressed()
    {
        OnPauseButtonPressed?.Invoke();
        timeCount.StopTimer();
        pauseMenu.gameObject.SetActive(true);
        SoundManager.Instance.PlayClickSound();
        PauseGame();
    }
    private void OnUIResumeButtonPressed()
    {
        OnResumeButtonPressed?.Invoke();
        timeCount.StartCountdown();
        pauseMenu.gameObject.SetActive(false);
        SoundManager.Instance.PlayClickSound();
        ResumeGame();
    }

    private void OnUIRetryButtonPressed()
    {
        OnRetryButtonPressed?.Invoke();
        heart.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        timeCountUi.gameObject.SetActive(false);
        flashlight.gameObject.SetActive(false);
        winLoseUi.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        scanText.gameObject.SetActive(true);
        timeCount.ResetTimer();
        SoundManager.Instance.PlayClickSound();
        ResumeGame();
    }
    
    public void onGameWinShown()
    {
        winLoseUi.gameObject.SetActive(true);
        winUiShown.gameObject.SetActive(true);
        loseUiShown.gameObject.SetActive(false);
        SoundManager.Instance.PlayWinSound();
        PauseGame();
    }

    public void onGameLoseShown()
    {
        winLoseUi.gameObject.SetActive(true);
        loseUiShown.gameObject.SetActive(true);
        winUiShown.gameObject.SetActive(false);
        SoundManager.Instance.PlayLoseSound();
        PauseGame();
    } 

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPause = false;
    }
}
