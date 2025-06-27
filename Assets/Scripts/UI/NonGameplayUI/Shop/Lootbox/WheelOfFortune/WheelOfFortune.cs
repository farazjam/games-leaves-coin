using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public enum SpinResultType
{
    None = -1,
    Loss,
    Win
};

public class WheelOfFortune : BasePopup
{
    [SerializeField] private WheelSpinner wheelSpinner;
    [SerializeField] private List<WheelSliceItem> wheelSliceItems;
    [SerializeField] private Button SpinButton;

    private int spinCount = 0;
    private int userChoiceNumber = 0;
    private bool isUserSelectedSlice = false;
    private bool spinLock = false;
    private bool isWinningSpin = false;
    private readonly int winPercentageBeyondDefined = 5;
    private readonly int winReward = 8;

    private void Start()
    {
        AssertUtil.IsNotNull(wheelSliceItems);
        AssertUtil.AreEqual(wheelSliceItems.Count, 8);
        AssertUtil.IsNotNull(SpinButton);

        for (int i = 0; i < wheelSliceItems.Count; i++)
        {
            var num = i;
            wheelSliceItems[i].Init(() => UserChooseNumber(num));
        }

        SpinButton.onClick.AddListener(() =>
        {
            if (!isUserSelectedSlice)
            {
                Debug.Log("Select a slice first, before spinning");
                return;
            }
            CreditCheckForSpin(StartSpin);
        });
    }

    protected override void Subscribe()
    {
        EventBus.Shop.WheelOfFortune.OnOpenned += Show;
    }

    protected override void Unsubscribe()
    {
        EventBus.Shop.WheelOfFortune.OnOpenned -= Show;
    }

    private void CreditCheckForSpin(Action successCallback)
    {
        EventBus.Shop.WheelOfFortune.SpinRequest((bool canSpin) => 
        {
            if (!canSpin) return;

            Debug.Log("SpinRequest successful, credit amount available, deducted");
            successCallback?.Invoke();
        });
    }

    private void StartSpin()
    {
        if (spinLock) return;
        spinLock = true;
        SpinButton.interactable = false;

        isWinningSpin = IsWinningSpin();

        wheelSpinner.Spin(
        selectedIndex: userChoiceNumber,
        isWinningSpin: isWinningSpin,
        onComplete: () =>
        {
            isUserSelectedSlice = false;
            spinLock = false;
            SpinButton.interactable = true;
            wheelSliceItems.ForEach(sliceItem => sliceItem.Select(false));
            SpinningComplete();
        });
    }

    private void SpinningComplete()
    {
        spinCount++;
        Action callback = isWinningSpin switch
        {
            true => () =>
            {
                EventBus.Shop.WheelOfFortune.CompleteSpin(winReward);
                Debug.Log($"Won, rewards = {winReward}");
            }
            ,
            false => () =>
            {
                Debug.Log("No win, no reward");
            },
        };
        callback?.Invoke();
    }

    private void UserChooseNumber(int number)
    {
        userChoiceNumber = number;
        isUserSelectedSlice = true;
        Debug.Log($"userChoiceNumber : {userChoiceNumber}");

        wheelSliceItems
        .Select((item, index) => new { item, index })
        .ToList()
        .ForEach(sliceItem => sliceItem.item.Select(sliceItem.index == number));
    }

    private bool IsWinningSpin()
    {
        var nextSpinCount = spinCount + 1;
        var spinResult = nextSpinCount switch
        {
            1 => SpinResultType.Loss,
            2 => SpinResultType.Win,
            3 => SpinResultType.Loss,
            4 => SpinResultType.Loss,
            5 => SpinResultType.Loss,
            6 => SpinResultType.Loss,
            7 => SpinResultType.Win,
            8 => SpinResultType.Win,
            9 => SpinResultType.Loss,
            10 => SpinResultType.Loss,
            _ => UnityEngine.Random.value < (winPercentageBeyondDefined / 100f) ? SpinResultType.Win : SpinResultType.Loss
        };
       
        var isWinningSpin = (spinResult == SpinResultType.Win);
        Debug.Log($"IsWinningSpin - spinCount:{spinCount}, isWinningSpin:{isWinningSpin}");
        return isWinningSpin;
    }

    public override void OnCloseButtonPressed()
    {
        if (spinLock) return;
        base.OnCloseButtonPressed();
    }
}
