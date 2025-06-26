using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public interface ILoadableManager
{
    bool IsReady { get; }
    void Init();
}

public abstract class LoadableManager<T> : Singleton<T>, ILoadableManager where T : MonoBehaviour
{
    public bool IsReady { get; protected set; }

    public void Init()
    {
        StopCoroutine(nameof(InitRoutineWrapper));
        StartCoroutine(nameof(InitRoutineWrapper));
    }

    private IEnumerator InitRoutineWrapper()
    {
        yield return InitRoutine();
        IsReady = true;
    }

    protected virtual IEnumerator InitRoutine()
    {
        yield return null;
    }
}

[Serializable]
public class Loader : Singleton<Loader>
{
    [SerializeField] private List<MonoBehaviour> managerObjects;
    private readonly List<ILoadableManager> managers = new();

    private void Start()
    {
        managerObjects.ForEach(managerObject =>
        {
            AssertUtil.IsNotNull(managerObject);
            if (managerObject is ILoadableManager loadableManager)
            {
                managers.Add(loadableManager);
            }
            else
            {
                Debug.LogWarning($"{managerObject.name} doesn't implement ILoadableManager.");
            }
        });

        StopCoroutine(nameof(LoadManagersSequentially));
        StartCoroutine(nameof(LoadManagersSequentially));
    }

    private IEnumerator LoadManagersSequentially()
    {
        foreach (var manager in managers)
        {
            manager.Init();
            while (!manager.IsReady)
            {
                yield return null;
            }
            Debug.Log($"{manager.GetType().Name} is loaded");
        }

        Debug.Log("All loadable managers are loaded");
        EventBus.Loader.LoadingCompleted();
    }
}

