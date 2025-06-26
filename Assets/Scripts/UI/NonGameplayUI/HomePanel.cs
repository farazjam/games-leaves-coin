using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePanel : BasePopup
{
    private readonly ActionTabType actionTabType = ActionTabType.Home;

    protected override void Init()
    {

    }

    protected override void Subscribe()
    {
        EventBus.UI.OnActionTabPressed += OnActionTabPressed;
    }

    protected override void Unsubscribe()
    {
        EventBus.UI.OnActionTabPressed -= OnActionTabPressed;
    }

    private void OnActionTabPressed(ActionTabType actionTabType)
    {
        var thisType = actionTabType == this.actionTabType;
        SetActive(thisType);
    }
}
