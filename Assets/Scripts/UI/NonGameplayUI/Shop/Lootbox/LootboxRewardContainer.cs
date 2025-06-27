using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum LootboxRewardItemType
{
    None,
    WheelOfFortune,
}

[Serializable]
public class LootboxRewardItemArg
{
    public LootboxRewardItemType Type;
    public LootboxRewardItem LootboxRewardItem;
    [HideInInspector] public Action ButtonPressedCallback;
}

public class LootboxRewardContainer : MonoBehaviour
{
    [SerializeField] private List<LootboxRewardItemArg> lootboxRewardItemArgs;
    private readonly Dictionary<LootboxRewardItemType, LootboxRewardItemArg> lootboxRewardItemArgsDict = new();
    private readonly Dictionary<LootboxRewardItemType, Action> lootboxRewardItemButtonCallbacks = new()
    {
        { LootboxRewardItemType.WheelOfFortune, () => EventBus.Shop.WheelOfFortune.Open() },
    };

    public void Init()
    {
        lootboxRewardItemArgs.ForEach(lootboxRewardItemArg =>
        {
            AssertUtil.IsNotNull(lootboxRewardItemArg);
            AssertUtil.AreNotEqual(lootboxRewardItemArg.Type, LootboxRewardItemType.None);
            AssertUtil.IsNotNull(lootboxRewardItemArg.LootboxRewardItem);

            lootboxRewardItemArg.ButtonPressedCallback = lootboxRewardItemButtonCallbacks?[lootboxRewardItemArg.Type] ?? null;
            lootboxRewardItemArgsDict.TryAdd(lootboxRewardItemArg.Type, lootboxRewardItemArg);
            lootboxRewardItemArg.LootboxRewardItem.Init(lootboxRewardItemArg);
        });
    }

    
}
