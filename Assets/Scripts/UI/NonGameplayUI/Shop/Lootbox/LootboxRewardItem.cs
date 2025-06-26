using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootboxRewardItem : MonoBehaviour
{
    [SerializeField] private LootboxRewardItemType type;
    [SerializeField] private Button button;
    private LootboxRewardItemArg lootboxRewardItemArg;

    public void Init(LootboxRewardItemArg lootboxRewardItemArg)
    {
        AssertUtil.IsNotNull(lootboxRewardItemArg);
        AssertUtil.AreNotEqual(type, LootboxRewardItemType.None);
        AssertUtil.AreEqual(type, lootboxRewardItemArg.Type);
        AssertUtil.IsNotNull(button);

        this.lootboxRewardItemArg = lootboxRewardItemArg;
        button.onClick.AddListener(() => Debug.Log($"LootboxRewardItem Type: {lootboxRewardItemArg.Type}"));
    }
}
