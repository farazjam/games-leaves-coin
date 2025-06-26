using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum GameState
{
    None,
    Init,
    MainMenu,
    Gameplay
}

public interface IGameStates
{
    public void OnInit();
    public void OnMainMenu();
    public void OnGameplay();
}

[Serializable]
public class GameManager : LoadableManager<GameManager>
{
    public static event Action<GameState> OnGameStateChanged;

    private GameState state;
    public GameState State 
    { 
        get => state;
        set
        {
            if (state == value)
            {
                Debug.LogError($"GameState change rejected. Both are same - Current: {State} => New: {value}");
                return;
            }

            if(!ValidateStateChange(value))
            {
                Debug.LogError($"GameState change rejected. Current: {State} => New: {value}");
                return;
            }

            state = value;
            OnGameStateChanged?.Invoke(state);
            OnStateChanged();
        }
    }

    private void OnEnable()
    {
        EventBus.Loader.OnLoadingComplete += OnLoadingComplete;
    }

    private void OnDisable()
    {
        EventBus.Loader.OnLoadingComplete -= OnLoadingComplete;
    }

    private void OnLoadingComplete()
    {
        StopCoroutine(nameof(OnAfterLoadingComplete));
        StartCoroutine(nameof(OnAfterLoadingComplete));
    }

    private IEnumerator OnAfterLoadingComplete()
    {
        State = GameState.Init;
        yield return null;
    }

    private bool ValidateStateChange(GameState newGameState)
    {
        return (state, newGameState) switch
        {
            (GameState.None, GameState.Init) or
            (GameState.Init, GameState.MainMenu) or
            (GameState.MainMenu, GameState.Gameplay) or
            (GameState.Gameplay, GameState.MainMenu)
                => true,
            _ => false
        };
    }

    private void OnStateChanged()
    {
        Action action = state switch
        {
            GameState.Init => () => 
            {
                State = GameState.MainMenu;
            },
            GameState.MainMenu => () =>
            {
                SceneManagement.Instance.LoadSceneAsync(SceneType.Main, 
                    () => NonGameplayUIManager.Instance.OnInit());
            },
            GameState.Gameplay => () =>
            {
                SceneManagement.Instance.LoadSceneAsync(SceneType.Gameplay, 
                    () => GameplayUIManager.Instance.OnInit());
            },
            _ => null
        };
        Debug.Log($"GameManager.OnStateChanged: {State}");
        action?.Invoke();
    }

    
}
