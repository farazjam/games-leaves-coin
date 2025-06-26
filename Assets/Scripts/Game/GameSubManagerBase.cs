using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSubManagerBase<T> : Singleton<T>, IGameStates where T : MonoBehaviour
{
    protected GameState State;
    protected bool isInit;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnStateChanged;
        Subscribe();
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnStateChanged;
        Unsubscribe();
    }

    private void OnStateChanged(GameState state)
    {
        State = state;
        OnGameStateChanged();

        Action action = state switch
        {
            GameState.Init => () => OnInit(),
            GameState.MainMenu => () => OnMainMenu(),
            GameState.Gameplay => () => OnGameplay(),
            _ => null
        };
        action?.Invoke();
    }

    public void OnInit()
    {
        if (isInit) return;
        isInit = true;

        Debug.Log($"{typeof(T).Name} Initialized");
        Init();
    }

    protected abstract void Init();
    public virtual void OnMainMenu() { }
    public virtual void OnGameplay() { }

    public virtual void OnGameStateChanged() { }
    protected virtual void Subscribe() { }
    protected virtual void Unsubscribe() { }
}
