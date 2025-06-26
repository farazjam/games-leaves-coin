using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


[Serializable]
public class HudCurrencyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyValueText;
    [SerializeField] private Image currencyIconImage;
    [SerializeField] private Button currencyButton;

    private CurrencyArg currencyArg;

    public void Validate()
    {
        AssertUtil.IsNotNull(currencyValueText);
        AssertUtil.IsNotNull(currencyIconImage);
        AssertUtil.IsNotNull(currencyButton);
    }

    public void Init(CurrencyArg currencyArg)
    {
        if(currencyArg == null)
        {
            Debug.LogError($"CurrencyArg is null : {currencyArg}");
            return;
        }
        
        this.currencyArg = currencyArg;
        currencyValueText.text = currencyArg.Amount.ToString();
        currencyIconImage.sprite = ConfigManager.Instance.SpriteConfig.GetSprite(currencyArg.Icon);
        currencyButton.onClick.AddListener(() => 
        { 
            EventBus.UI.ShopOpen(currencyArg.Type); 
        });
    }

    void OnEnable()
    {
        EventBus.Shop.OnCurrencyUpdated += OnCurrencyUpdated;
    }

    void OnDisable()
    {
        EventBus.Shop.OnCurrencyUpdated -= OnCurrencyUpdated;
    }

    private void OnCurrencyUpdated(CurrencyType currencyType, long amount)
    {
        if (this.currencyArg.Type != currencyType) return;
        currencyValueText.text = amount.ToString();
    }
}
