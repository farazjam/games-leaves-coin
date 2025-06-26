using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[Serializable]
public class NonGameplayUIPanelArg
{
    public ActionTabType Type;
    public BasePopup BasePopup;
    public Transform ParentTransform;
}


public class NonGameplayUIManager  : GameSubManagerBase<NonGameplayUIManager>
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private ActionTabsbarContainer actionTabsbarContainer;
    [SerializeField] private List<NonGameplayUIPanelArg> nonGameplayUIPanelArgs;
    private readonly Dictionary<ActionTabType, NonGameplayUIPanelArg> nonGameplayUIPanelArgsDict = new();

    private bool ThisGameState => State == GameState.MainMenu;
    private ActionTabType currentPanelType;

    protected override void Init()
    {
        AssertUtil.IsNotNull(parentTransform);
        AssertUtil.IsNotNull(actionTabsbarContainer);
        AssertUtil.IsNotNull(nonGameplayUIPanelArgs);

        int panelEnumLength = EnumExtensions.GetEnumLength<ActionTabType>() - 1;
        AssertUtil.AreEqual(panelEnumLength, nonGameplayUIPanelArgs.Count,
            $"nonGameplayUIPanelArgs count mismatched. panelEnumLength: {panelEnumLength} != nonGameplayUIPanelArgs.Count: {nonGameplayUIPanelArgs.Count}");

        nonGameplayUIPanelArgs.ForEach(panel =>
        {
            AssertUtil.IsNotNull(panel);
            AssertUtil.IsNotNull(panel.BasePopup);
            AssertUtil.IsNotNull(panel.ParentTransform);
            nonGameplayUIPanelArgsDict.TryAdd(panel.Type, panel);
            panel.BasePopup.transform.gameObject.SetActive(true);
            panel.BasePopup.Hide();
        });

        parentTransform.gameObject.SetActive(true);
    }

    protected override void Subscribe()
    {
        EventBus.UI.OnActionTabPressed += OnActionTabPressed;
    }

    protected override void Unsubscribe()
    {
        EventBus.UI.OnActionTabPressed -= OnActionTabPressed;
    }

    public override void OnMainMenu()
    {
        
    }

    public override void OnGameplay()
    {
        
    }

    public override void OnGameStateChanged()
    {
        parentTransform.gameObject.SetActive(ThisGameState);
    }

    private void OnActionTabPressed(ActionTabType newPanelType)
    {
        Action callback = currentPanelType switch
        {
            ActionTabType.None => () =>
            {
                currentPanelType = newPanelType;
            },

            ActionTabType.Player or ActionTabType.Shop => () =>
            {
                if (newPanelType == ActionTabType.Home)
                {
                    nonGameplayUIPanelArgsDict?[newPanelType].BasePopup.Hide();
                    currentPanelType = newPanelType;
                }
                else
                {
                    currentPanelType = newPanelType;
                }
            },

            ActionTabType.Home => () =>
            {
                if (newPanelType == ActionTabType.Home)
                {
                    GameManager.Instance.State = GameState.Gameplay;
                }
                currentPanelType = newPanelType;
            },
            _ => null
        };

        callback?.Invoke();
    }
}
