using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HudBar : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private HudCurrencyPanelsParent hudCurrencyPanelsParent;

    void Start()
    {
        AssertUtil.IsNotNull(settingsButton);
        AssertUtil.IsNotNull(hudCurrencyPanelsParent);

        settingsButton.onClick.AddListener(() => { Debug.Log("Settings"); });
        hudCurrencyPanelsParent.Init();
    }
}
