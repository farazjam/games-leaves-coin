using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CurrencySpendPanelTemp
{
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private Button dummySpendButton1;
    [SerializeField] private Button dummySpendButton10;
    [SerializeField] private Button dummySpendButton100;

    public void Init()
    {
        AssertUtil.AreNotEqual(currencyType, CurrencyType.None);
        AssertUtil.IsNotNull(dummySpendButton1);
        AssertUtil.IsNotNull(dummySpendButton10);
        AssertUtil.IsNotNull(dummySpendButton100);

        dummySpendButton1.onClick.AddListener(() => Spend(1));
        dummySpendButton10.onClick.AddListener(() => Spend(10));
        dummySpendButton100.onClick.AddListener(() => Spend(100));
    }

    private void Spend(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log($"Invalid spend amount : {amount}");
            return;
        }

        Debug.Log($"Spend Request - {currencyType}, Amount: {amount}");
        EventBus.Shop.CurrencySpendRequested(currencyType, amount);
    }
}

/// <summary>
/// Acts as dummy spend panel
/// </summary>
public class PlayerPanel : BasePopup
{
    [SerializeField] private List<CurrencySpendPanelTemp> currencySpendPanels;
    private readonly ActionTabType actionTabType = ActionTabType.Player;

    protected override void Init()
    {
        AssertUtil.IsNotNull(currencySpendPanels);
        currencySpendPanels.ForEach(currencySpendPanel =>
        {
            AssertUtil.IsNotNull(currencySpendPanel);
            currencySpendPanel.Init();
        });
    }

    protected override void Subscribe()
    {
        EventBus.UI.OnActionTabPressed += OnActionTabPressed;
    }

    protected override void Unsubscribe()
    {
        EventBus.UI.OnActionTabPressed -= OnActionTabPressed;
    }

    private void OnActionTabPressed(ActionTabType actionTabType)
    {
        var thisType = actionTabType == this.actionTabType;
        SetActive(thisType);
    }
}
