using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Threading.Tasks;

[Serializable]
public class DailyCurrencyRewardItem : MonoBehaviour
{
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image currencyIcon;
    [SerializeField] private Button Button;
    [SerializeField] private CountdownTimerPanel countdownTest;

    private CurrencyArgDailyReward currencyArgDailyReward;
    private readonly UtcTimeService utcTimeService = new();

    public CurrencyType CurrencyType => currencyType;

    private void Validate()
    {
        AssertUtil.AreNotEqual(currencyType, CurrencyType.None);
        AssertUtil.IsNotNull(amountText);
        AssertUtil.IsNotNull(currencyIcon);
        AssertUtil.IsNotNull(Button);
        AssertUtil.IsNotNull(countdownTest);
    }

    public void Init(CurrencyArgDailyReward currencyArgDailyReward)
    {
        if (currencyArgDailyReward == null || currencyArgDailyReward.Type != currencyType) return;

        Validate();

        this.currencyArgDailyReward = currencyArgDailyReward;
        amountText.text = currencyArgDailyReward.Amount.ToString();
        currencyIcon.sprite = ConfigManager.Instance.SpriteConfig.GetSprite(currencyArgDailyReward.Icon);
        Button.onClick.AddListener(() => EventBus.Shop.RequestDailyReward(currencyType, callback: HandleClaimUI));
        Button.interactable = true;

        HandleClaimUI();
    }

    private async void HandleClaimUI()
    {
        if (currencyArgDailyReward.ResetHourUTC24 <= 0) return;

        TimeSpan? remainingTime = await utcTimeService.GetTimeDifferenceFromNowAsync(currencyArgDailyReward.NextClaim);
        if (remainingTime <= TimeSpan.Zero)
        {
            // Claim is available
            Button.interactable = true;
        }
        else
        {
            // Claim unavailable, show countdown
            Button.interactable = false;
            countdownTest.StartTimer(Mathf.Abs((float)remainingTime.Value.TotalSeconds), OnTimerFinished: HandleClaimUI);
        }
    }
}
