using System.Collections;
using UnityEngine;
using TMPro;

public class TimeCountDown : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private float timeInSeconds = 180f;

    private bool isRunning = false;
    private float remainingTime;
    private Coroutine countdownCoroutine;

    public void StartCountdown()
    {
        if (!isRunning)
        {
            isRunning = true;
            if(remainingTime <= 0f || remainingTime > timeInSeconds)
                remainingTime = timeInSeconds;

            countdownCoroutine = StartCoroutine(CountdownRoutine());
        }
    }

    private IEnumerator CountdownRoutine()
    {
        while (remainingTime > 0 && isRunning)
        {
            UpdateTimerDisplay(remainingTime);
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        UpdateTimerDisplay(0);

        if (remainingTime <= 0)
        {
            uiManager.onGameWinShown();
        }
        isRunning = false;
    }

    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        isRunning = false;
        if(countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    public void ResetTimer()
    {
        StopTimer();
    remainingTime = timeInSeconds;
UpdateTimerDisplay(remainingTime);
    }
}
