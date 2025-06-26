using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigManager : LoadableManager<ConfigManager>
{
    [SerializeField] private CurrencyConfig currencyConfig;
    [SerializeField] private DailyRewardConfig dailyRewardConfig;
    [SerializeField] private SpriteConfig spriteConfig;
    private PlayerData playerData;

    public CurrencyConfig CurrencyConfig => currencyConfig;
    public DailyRewardConfig DailyRewardConfig => dailyRewardConfig;
    public SpriteConfig SpriteConfig => spriteConfig;
    public PlayerData PlayerData => playerData.Clone();

    protected override IEnumerator InitRoutine()
    {
        AssertUtil.IsNotNull(currencyConfig);
        AssertUtil.IsNotNull(dailyRewardConfig);
        AssertUtil.IsNotNull(spriteConfig);

        yield return spriteConfig.LoadConfigFileAsync();
        yield return currencyConfig.LoadConfigFileAsync();
        yield return dailyRewardConfig.LoadConfigFileAsync();

        playerData = new(
            currencyArgs: currencyConfig.CurrencyArgsDict.Values.ToList(),
            currencyArgDailyRewards: dailyRewardConfig.CurrencyArgDailyRewardsDict.Values.ToList()
        );
    }
}
