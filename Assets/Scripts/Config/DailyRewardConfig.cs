using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DailyRewardConfig", menuName = "Config/DailyRewardConfig")]
public class DailyRewardConfig : AbstractConfig
{
    [SerializeField] private List<CurrencyArgDailyReward> currencyArgDailyRewardsList;
    private readonly Dictionary<CurrencyType, CurrencyArgDailyReward> currencyArgDailyRewardsDict = new();

    public IReadOnlyDictionary<CurrencyType, CurrencyArgDailyReward> CurrencyArgDailyRewardsDict => currencyArgDailyRewardsDict;

    protected override void LoadConfigFile()
    {
        if(currencyArgDailyRewardsList.Count <= 0)
        {
            Debug.LogWarning($"currencyArgDailyRewardsList is empty");
        }

        currencyArgDailyRewardsDict.Clear();
        foreach (var currencyArgDailyReward in currencyArgDailyRewardsList)
        {
            AssertUtil.IsNotNull(currencyArgDailyReward);
            AssertUtil.IsTrue(currencyArgDailyReward.Amount > 0, "Reward value cannot be < 0");

            if(String.IsNullOrEmpty(currencyArgDailyReward.LastClaim))
            {
                currencyArgDailyReward.LastClaim = currencyArgDailyReward.NextClaim = DateTime.MinValue.ToString("o");
            }

            if (String.IsNullOrEmpty(currencyArgDailyReward.NextClaim))
            {
                currencyArgDailyReward.NextClaim = DateTime.MinValue.ToString("o");
            }

            if (!currencyArgDailyRewardsDict.TryAdd(currencyArgDailyReward.Type, currencyArgDailyReward))
            {
                Debug.LogError($"Unable to add currencyArgDailyReward type : {currencyArgDailyReward.Type}");
            }
        }
    }

    public CurrencyArgDailyReward GetCurrencyDailyRewardConfig(CurrencyType currencyType)
    {
        if (currencyType == CurrencyType.None)
        {
            Debug.LogError($"Invalid reward currency type: {currencyType}");
            return null;
        }

        if (currencyArgDailyRewardsDict == null)
        {
            Debug.LogError($"CurrencyDailyRewardArgsDict is null, currencyDailyRewardArgsDict.Count: {currencyArgDailyRewardsDict?.Count ?? 0}");
            return null;
        }

        if (currencyArgDailyRewardsDict.TryGetValue(currencyType, out var currencyArg))
        {
            return currencyArg;
        }

        Debug.LogError($"CurrencyDailyRewardArgsDict not found for CurrencyType: {currencyType}");
        return null;
    }

    public IEnumerable<CurrencyArgDailyReward> GetAllCurrencyConfigs()
    {
        foreach (var currencyDailyRewardArg in CurrencyArgDailyRewardsDict.Values)
        {
            yield return currencyDailyRewardArg;
        }
    }
}
