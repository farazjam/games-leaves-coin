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
}

public class LootboxRewardContainer : MonoBehaviour
{
    [SerializeField] private List<LootboxRewardItemArg> lootboxRewardItemArgs;

    public void Init()
    {
        lootboxRewardItemArgs.ForEach(lootboxRewardItemArg =>
        {
            AssertUtil.IsNotNull(lootboxRewardItemArg);
            lootboxRewardItemArg.LootboxRewardItem.Init(lootboxRewardItemArg);
        });
    }
}
