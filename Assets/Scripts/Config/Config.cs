using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Config/Config")]
public class Config : ScriptableObject
{
    public CurrencyConfig currencyConfig;
    public DailyRewardConfig dailyRewardConfig;
    public SpriteConfig spriteConfig;
    public WheelOfFortuneConfig wheelOfFortuneConfig;

    private void OnEnable()
    {
        AssertUtil.IsNotNull(currencyConfig);
        AssertUtil.IsNotNull(dailyRewardConfig);
        AssertUtil.IsNotNull(spriteConfig);
        AssertUtil.IsNotNull(wheelOfFortuneConfig);
    }
}
