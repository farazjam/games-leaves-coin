using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class ShopPanel : BasePopup
{
    [SerializeField] private DailyCurrencyRewardContainer DailyCurrencyRewardContainer;
    [SerializeField] private LootboxRewardContainer LootboxRewardContainer;
    private readonly ActionTabType actionTabType = ActionTabType.Shop;

    protected override void Init()
    {
        AssertUtil.IsNotNull(DailyCurrencyRewardContainer);
        AssertUtil.IsNotNull(LootboxRewardContainer);

        DailyCurrencyRewardContainer.Init();
        LootboxRewardContainer.Init();
    }

    protected override void Subscribe()
    {
        EventBus.UI.OnShopOpen += OpenShop;
        EventBus.UI.OnActionTabPressed += OnActionTabPressed;
    }

    protected override void Unsubscribe()
    {
        EventBus.UI.OnShopOpen -= OpenShop;
        EventBus.UI.OnActionTabPressed -= OnActionTabPressed;
    }

    private void OpenShop(CurrencyType currencyType)
    {
        Action action = currencyType switch
        {
            CurrencyType.Coin => () =>
            {
                Show();
            },
            _ => () =>
            {
                Debug.LogWarning($"ShopPanel.OpenShop - CurrencyType: {currencyType} Not Implemented.");
            }
        };
        action?.Invoke();
    }

    private void OnActionTabPressed(ActionTabType actionTabType)
    {
        var thisType = actionTabType == this.actionTabType;
        SetActive(thisType);
    }
}
