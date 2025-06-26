using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : GameSubManagerBase<GameplayUIManager>
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Button gameplayQuitButton;

    private bool ThisGameState => State == GameState.Gameplay;

    protected override void Init()
    {
        AssertUtil.IsNotNull(parentTransform);
        AssertUtil.IsNotNull(gameplayQuitButton);

        gameplayQuitButton.onClick.AddListener(() =>
        {
            Debug.Log("Quit gameplay");
            GameManager.Instance.State = GameState.MainMenu;
        });

        parentTransform.gameObject.SetActive(true);
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
}

