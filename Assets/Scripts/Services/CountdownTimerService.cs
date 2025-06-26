using System;
using System.Threading.Tasks;
using UnityEngine;

public class CountdownTimerService
{
    private float countdownTime;
    private bool isRunning;
    private float lastUpdateTime;
    private readonly int delay = 1000;

    public event Action<float> OnTimeUpdated;
    public event Action OnTimerFinished;

    public CountdownTimerService(float countdownTime)
    {
        this.countdownTime = countdownTime + Time.realtimeSinceStartup;
    }

    public async void StartTimer()
    {
        if (isRunning || countdownTime <= 0) return;
        isRunning = true;

        while (countdownTime > 0)
        {
            await Task.Delay(delay);
            if (isRunning)
            {
                float currentTime = Time.realtimeSinceStartup;
                var countdownTimeCached = countdownTime;
                countdownTime -= (currentTime - lastUpdateTime);
                lastUpdateTime = Time.realtimeSinceStartup;
                OnTimeUpdated?.Invoke(Mathf.Max(countdownTime, 0));
            }
        }

        if (isRunning)
        {
            Debug.Log($"CountdownTimerService.StartTimer - OnTimerFinished: {countdownTime}");
            OnTimerFinished?.Invoke();
        }

        Debug.Log($"CountdownTimerService.StartTimer - isRunning: {isRunning}");
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log($"CountdownTimerService.StopTimer");
    }

    public void ResetTimer(float newCountdownTime)
    {
        countdownTime = newCountdownTime;
        Debug.Log($"CountdownTimerService.ResetTimer {countdownTime}");
    }

    public void ResumeTimer()
    {
        isRunning = false;
        StartTimer();
        Debug.Log($"CountdownTimerService.ResumeTimer - lastUpdateTime: {lastUpdateTime}");
    }

    public void PauseTimer()
    {
        isRunning = false;
        Debug.Log($"CountdownTimerService.PauseTimer");
    }
}

