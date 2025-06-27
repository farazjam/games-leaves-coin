using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigManager : LoadableManager<ConfigManager>
{
    [SerializeField] private Config config;
    private PlayerData playerData;

    public CurrencyConfig CurrencyConfig => config.currencyConfig;
    public DailyRewardConfig DailyRewardConfig => config.dailyRewardConfig;
    public SpriteConfig SpriteConfig => config.spriteConfig;
    public WheelOfFortuneConfig WheelOfFortuneConfig => config.wheelOfFortuneConfig;
    public PlayerData PlayerData => playerData.Clone();

    protected override IEnumerator InitRoutine()
    {
        AssertUtil.IsNotNull(config);
        AssertUtil.IsNotNull(config.currencyConfig);
        AssertUtil.IsNotNull(config.dailyRewardConfig);
        AssertUtil.IsNotNull(config.spriteConfig);
        AssertUtil.IsNotNull(config.wheelOfFortuneConfig);

        yield return config.spriteConfig.LoadConfigFileAsync();
        yield return config.currencyConfig.LoadConfigFileAsync();
        yield return config.dailyRewardConfig.LoadConfigFileAsync();
        yield return config.wheelOfFortuneConfig.LoadConfigFileAsync();

        playerData = new(
            currencyArgs: config.currencyConfig.CurrencyArgsDict.Values.ToList(),
            currencyArgDailyRewards: config.dailyRewardConfig.CurrencyArgDailyRewardsDict.Values.ToList(),
            wheelOfFortuneCurrencyArg: config.wheelOfFortuneConfig.CurrencyArg
        );
    }
}
