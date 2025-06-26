using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class HudCurrencyPanelArg
{
    public CurrencyType CurrencyType;
    public HudCurrencyPanel HudCurrencyPanel;

    public void Validate()
    {
        AssertUtil.AreNotEqual(CurrencyType, CurrencyType.None);
        AssertUtil.IsNotNull(HudCurrencyPanel);
        HudCurrencyPanel.Validate();
    }
}

public class HudCurrencyPanelsParent : MonoBehaviour
{
    [SerializeField] private List<HudCurrencyPanelArg> hudCurrencyPanelArgs;
    private readonly Dictionary<CurrencyType, HudCurrencyPanelArg> hudCurrencyPanelArgsDict = new();

    public void Init()
    {
        AssertUtil.IsNotNull(hudCurrencyPanelArgs);

        hudCurrencyPanelArgs.ForEach(hudCurrencyPanelArg => 
        {
            var currencyArg = DataManager.Instance.GetCurrencyArg(hudCurrencyPanelArg.CurrencyType);
            if(currencyArg == null)
            {
                Debug.LogError($"Currency Arg is null: {hudCurrencyPanelArg.CurrencyType}");
                return;
            }
            hudCurrencyPanelArgsDict.TryAdd(hudCurrencyPanelArg.CurrencyType, hudCurrencyPanelArg);
            hudCurrencyPanelArg.HudCurrencyPanel.Init(currencyArg); 
        });
    }

}
