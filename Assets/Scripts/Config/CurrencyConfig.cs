using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "Config", menuName = "Config/CurrencyConfig")]
public class CurrencyConfig : AbstractConfig
{
    [SerializeField] private List<CurrencyArg> currencyArgsList;
    private readonly Dictionary<CurrencyType, CurrencyArg> currencyArgsDict = new();

    public IReadOnlyDictionary<CurrencyType, CurrencyArg> CurrencyArgsDict => currencyArgsDict;

    protected override void LoadConfigFile()
    {
        currencyArgsDict.Clear();
        foreach (var currencyArg in currencyArgsList)
        {
            AssertUtil.IsNotNull(currencyArg);
            AssertUtil.AreNotEqual(currencyArg.Type, CurrencyType.None);
            AssertUtil.IsTrue(currencyArg.Amount >= 0, "Value cannot be < 0");

            if (!currencyArgsDict.TryAdd(currencyArg.Type, currencyArg))
            {
                Debug.LogError($"Unable to add currency type : {currencyArg.Type}");
            }
        }

        int currencyEnumLength = EnumExtensions.GetEnumLength<CurrencyType>();
        Assert.AreNotEqual(currencyEnumLength, currencyArgsDict.Count, 
            $"Currency count mismatched. currencyEnumLength: {currencyEnumLength} != currencyDict.Count: {currencyArgsDict.Count}");
    }

}
