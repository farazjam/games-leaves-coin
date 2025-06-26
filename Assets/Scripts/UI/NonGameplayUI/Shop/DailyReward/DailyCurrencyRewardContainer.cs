using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyCurrencyRewardContainer : MonoBehaviour
{
    [SerializeField] private List<DailyCurrencyRewardItem> dailyCurrencyRewardItems;

    public void Init()
    {
        dailyCurrencyRewardItems.ForEach(dailyCurrencyRewardItem =>
        {
            AssertUtil.IsNotNull(dailyCurrencyRewardItem);

            var currencyType = dailyCurrencyRewardItem.CurrencyType;
            if (!DataManager.Instance.PlayerData.TryGetCurrencyDailyRewardArg(currencyType, out var currencDailyRewardyArg))
            {
                Debug.LogError($"currencDailyRewardyArg not found for type: {currencyType}");
                return;
            }

            dailyCurrencyRewardItem.Init(currencDailyRewardyArg);
        });
    }
}
