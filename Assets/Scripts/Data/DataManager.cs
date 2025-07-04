using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DataManager : LoadableManager<DataManager>
{
    private ConfigManager config;
    private PlayerData playerData;

    public PlayerData PlayerData => playerData;

    protected override IEnumerator InitRoutine()
    {
        config = ConfigManager.Instance;
        AssertUtil.IsNotNull(config);

        yield return null;
        LoadData();
    }

    private void LoadData()
    {
        var persistedPlayerData = ServiceLocator.Get<PersistenceService>().Load();
        if (persistedPlayerData != null)
        {
            Debug.Log($"DataManager.LoadPersistedData() - Persisted data loaded.");
            playerData = persistedPlayerData;
        }
        else
        {
            Debug.Log($"DataManager.LoadPersistedData() - Persisted data not available, using default config");
            playerData = config.PlayerData;
            ServiceLocator.Get<PersistenceService>().Save(playerData);
        }
    }

    private void OnEnable()
    {
        EventBus.Shop.OnDailyRewardRequested += DailyRewardRequested;
        EventBus.Shop.OnCurrencySpendRequested += CurrencySpendRequested;
        EventBus.Shop.WheelOfFortune.OnSpinRequested += WheelOfFortuneSpinRequested;
        EventBus.Shop.WheelOfFortune.OnSpinCompleted += WheelOfFortuneSpinCompleted;
    }

    private void OnDisable()
    {
        EventBus.Shop.OnDailyRewardRequested -= DailyRewardRequested;
        EventBus.Shop.OnCurrencySpendRequested -= CurrencySpendRequested;
        EventBus.Shop.WheelOfFortune.OnSpinRequested -= WheelOfFortuneSpinRequested;
        EventBus.Shop.WheelOfFortune.OnSpinCompleted -= WheelOfFortuneSpinCompleted;
    }

    public CurrencyArg GetCurrencyArg(CurrencyType currencyType)
    {
        if (!playerData.TryGetCurrencyArg(currencyType, out var currencyArg))
        {
            Debug.LogError($"CurrencyArg not found for type: {currencyType}");
            return null;
        }
        return currencyArg;
    }

    private void DailyRewardRequested(CurrencyType currencyType, Action callback)
    {
        // Get reward and amount
        if (!playerData.TryGetCurrencyDailyRewardArg(currencyType, out var currencDailyRewardyArg))
        {
            Debug.LogError($"currencDailyRewardyArg not found for type: {currencyType}");
            return;
        }
        var rewardAmount = currencDailyRewardyArg.Amount;

        // Add reward amount to currency
        var currencyArg = GetCurrencyArg(currencyType);
        if(currencyArg == null)
        {
            Debug.LogError($"CurrencyArg is null for type: {currencyType}");
            return;
        }
        currencyArg.Amount += rewardAmount;

        // Update claim values
        var lastClaimed = DateTime.UtcNow;
        currencDailyRewardyArg.LastClaim = lastClaimed.ToString("o");
        currencDailyRewardyArg.NextClaim = lastClaimed.Date.AddDays(1).AddHours(currencDailyRewardyArg.ResetHourUTC24).ToString("o");
        
        // Fire events
        EventBus.Shop.CurrencyUpdated(currencyType, currencyArg.Amount);
        ServiceLocator.Get<PersistenceService>().Save(playerData);
        callback?.Invoke();
    }

    private void CurrencySpendRequested(CurrencyType currencyType, long spendRequestAmount)
    {
        var currencyArg = GetCurrencyArg(currencyType);
        if (currencyArg == null)
        {
            Debug.LogError($"CurrencyArg is null for type: {currencyType}");
            return;
        }

        // Check credit
        var creditAmount = currencyArg.Amount;
        if(creditAmount < spendRequestAmount)
        {
            Debug.Log($"CreditAmount :{creditAmount} < SpendRequestAmount : {spendRequestAmount}. Transaction failed");
            return;
        }

        // Process transaction
        currencyArg.Amount -= spendRequestAmount;

        // Fire events
        EventBus.Shop.CurrencyUpdated(currencyType, currencyArg.Amount);
        ServiceLocator.Get<PersistenceService>().Save(playerData);
    }

    private void WheelOfFortuneSpinRequested(Action<bool> canSpin)
    {
        // Validate currency type of Wheel of fortune
        var wofCurrencyArg = playerData?.WheelOfFortuneCurrencyArg ?? null;
        if(wofCurrencyArg == null)
        {
            Debug.LogError($"WheelOfFortuneCurrencyArg is null for type: {wofCurrencyArg.Type}");
            return;
        }

        var currencyArg = GetCurrencyArg(wofCurrencyArg.Type);
        if (currencyArg == null)
        {
            Debug.LogError($"CurrencyArg is null for WheelOfFortuneCurrencyArg type: {currencyArg.Type}");
            return;
        }

        // Check credit
        var creditAmount = currencyArg.Amount;
        var spinCost = playerData.WheelOfFortuneCurrencyArg.Amount;
        if (creditAmount < spinCost)
        {
            Debug.Log($"CreditAmount :{creditAmount} < SpendRequestAmount : {spinCost}. Transaction failed");
            return;
        }

        // Process Transaction
        currencyArg.Amount -= spinCost;
        
        // Fire events
        EventBus.Shop.CurrencyUpdated(currencyArg.Type, currencyArg.Amount);
        ServiceLocator.Get<PersistenceService>().Save(playerData);
        canSpin?.Invoke(true);
    }

    private void WheelOfFortuneSpinCompleted(long spinRewardAmount)
    {
        // Validate currency type of Wheel of fortune
        var wofCurrencyArg = playerData?.WheelOfFortuneCurrencyArg ?? null;
        if (wofCurrencyArg == null)
        {
            Debug.LogError($"WheelOfFortuneCurrencyArg is null for type: {wofCurrencyArg.Type}");
            return;
        }

        // Add reward amount to currency
        var currencyArg = GetCurrencyArg(wofCurrencyArg.Type);
        if (currencyArg == null)
        {
            Debug.LogError($"CurrencyArg is null for type: {currencyArg.Type}");
            return;
        }
        currencyArg.Amount += spinRewardAmount;

        // Fire events
        EventBus.Shop.CurrencyUpdated(currencyArg.Type, currencyArg.Amount);
        ServiceLocator.Get<PersistenceService>().Save(playerData);
    }

}