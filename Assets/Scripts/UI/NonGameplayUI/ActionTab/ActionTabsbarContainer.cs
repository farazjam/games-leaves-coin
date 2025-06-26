using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public enum ActionTabType
{
    None,
    Player,
    Home,
    Shop
}


public class ActionTabsbarContainer : MonoBehaviour
{
    [SerializeField] private List<ActionTabItem> actionTabItems;

    void Start()
    {
        AssertUtil.IsNotNull(actionTabItems);

        int actionTabEnumLength = EnumExtensions.GetEnumLength<ActionTabType>() - 1;
        AssertUtil.AreEqual(actionTabEnumLength, actionTabItems.Count,
            $"Action tabs count mismatched. actionTabEnumLength: {actionTabEnumLength} != actionTabArgs.Count: {actionTabItems.Count}");

        actionTabItems.ForEach(actionTabItem => 
        {
            AssertUtil.IsNotNull(actionTabItem);
            actionTabItem.Init(onButtonClicked: (ActionTabType) =>
            {
                 EventBus.UI.ActionTabPressed(ActionTabType);
            });
        });
    }
}
