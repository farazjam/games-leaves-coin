using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private CountdownTimerService countdownTimerService;
    private float countdownTime;
    private event Action<float> OnTimeUpdated;
    private event Action OnTimerFinished;

    private void Awake()
    {
        AssertUtil.IsNotNull(timerText);
    }

    public void StartTimer(float value, Action OnTimerFinished = null, Action<float> OnTimeUpdated = null)
    {
        if (value <= 0) return;

        countdownTime = value;
        countdownTimerService = new(countdownTime);

        this.OnTimeUpdated = OnTimeUpdated;
        this.OnTimerFinished = OnTimerFinished;

        countdownTimerService.OnTimeUpdated += UpdateTimer;
        countdownTimerService.OnTimerFinished += StopTimer;

        countdownTimerService.StartTimer();
    }

    public void StopTimer()
    {
        if (countdownTimerService == null) return;
        countdownTimerService.StopTimer();
        UpdateTimer(0);
        this.OnTimerFinished?.Invoke();
    }

    private void UpdateTimer(float remainingSeconds)
    {
        int totalSeconds = Mathf.Max(0, Mathf.FloorToInt(remainingSeconds));

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        timerText.text = hours > 0
            ? $"{hours}:{minutes:D2}:{seconds:D2}"
            : $"{minutes}:{seconds:D2}";

        this.OnTimeUpdated?.Invoke(remainingSeconds);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (countdownTimerService == null) return;
        if (pauseStatus)
        {
            countdownTimerService.PauseTimer();
        }
        else
        {
            countdownTimerService.ResumeTimer();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (countdownTimerService == null) return;
        if (hasFocus)
        {
            countdownTimerService.ResumeTimer();
        }
        else
        {
            countdownTimerService.PauseTimer();
        }
    }

    private void OnDestroy()
    {
        if (countdownTimerService == null) return;
        countdownTimerService.OnTimeUpdated -= UpdateTimer;
        countdownTimerService.OnTimerFinished -= StopTimer;
    }

}