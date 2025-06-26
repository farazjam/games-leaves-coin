using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ActionTabItem : MonoBehaviour
{
    [SerializeField] private ActionTabType ActionTabType;
    [SerializeField] private Button Button;

    public void Init(Action<ActionTabType> onButtonClicked)
    {
        AssertUtil.AreNotEqual(ActionTabType, ActionTabType.None);
        AssertUtil.IsNotNull(Button);
        Button.onClick.AddListener(() => onButtonClicked?.Invoke(ActionTabType));
    }
}
