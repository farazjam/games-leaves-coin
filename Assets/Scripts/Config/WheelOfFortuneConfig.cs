using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "WheelOfFortuneConfig", menuName = "Config/WheelOfFortuneConfig")]
public class WheelOfFortuneConfig : AbstractConfig
{
    [SerializeField] private CurrencyArg currencyArg;

    public CurrencyArg CurrencyArg => currencyArg;

    protected override void LoadConfigFile()
    {
        AssertUtil.IsNotNull(currencyArg);
        AssertUtil.AreNotEqual(currencyArg.Type, CurrencyType.None);
        AssertUtil.IsTrue(currencyArg.Amount > 0, "Value cannot be < 0");
    }

}
