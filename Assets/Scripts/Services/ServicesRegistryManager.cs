using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only universal services needs to be registered
/// All instance specific services needs to be directly instantiated
/// </summary>
public class ServicesRegistryManager : LoadableManager<ServicesRegistryManager>
{
    protected override IEnumerator InitRoutine()
    {
        yield return null;
        RegisterServices();
    }

    private void RegisterServices()
    {
        ServiceLocator.Register<PersistenceService>(new());
    }
}