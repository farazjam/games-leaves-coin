using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class PlayerData
{
    [SerializeField] private List<CurrencyArg> currencyArgs = new();
    [SerializeField] private List<CurrencyArgDailyReward> currencyArgDailyRewards = new();

    public Dictionary<CurrencyType, CurrencyArg> CurrencyArgs => currencyArgs.ToDictionary(currencyArg => currencyArg.Type, currencyArgValue => currencyArgValue);
    public Dictionary<CurrencyType, CurrencyArgDailyReward> CurrencyArgDailyRewards => currencyArgDailyRewards.ToDictionary(currencyRewardArg => currencyRewardArg.Type, currencyRewardArgValue => currencyRewardArgValue);


    public PlayerData()
    {
    }

    public PlayerData(List<CurrencyArg> currencyArgs, List<CurrencyArgDailyReward> currencyArgDailyRewards)
    {
        if (currencyArgs != null)
        {
            foreach (var arg in currencyArgs)
            {
                this.currencyArgs.Add(arg.Clone());
            }
        }

        if (currencyArgDailyRewards != null)
        {
            foreach (var reward in currencyArgDailyRewards)
            {
                this.currencyArgDailyRewards.Add((CurrencyArgDailyReward)reward.Clone());
            }
        }
    }

    public PlayerData Clone()
    {
        var copy = new PlayerData();

        foreach (var arg in currencyArgs)
        {
            copy.currencyArgs.Add(arg.Clone());
        }

        foreach (var reward in currencyArgDailyRewards)
        {
            copy.currencyArgDailyRewards.Add((CurrencyArgDailyReward)reward.Clone());
        }

        return copy;
    }

    public bool TryGetCurrencyArg(CurrencyType type, out CurrencyArg arg)
    {
        if (CurrencyArgs == null)
        {
            throw new InvalidOperationException("CurrencyArgs not initialized.");
        }

        return CurrencyArgs.TryGetValue(type, out arg);
    }

    public bool TryGetCurrencyDailyRewardArg(CurrencyType type, out CurrencyArgDailyReward arg)
    {
        if (CurrencyArgDailyRewards == null)
        {
            throw new InvalidOperationException("CurrencyArgDailyRewards not initialized.");
        }

        return CurrencyArgDailyRewards.TryGetValue(type, out arg);
    }
}

