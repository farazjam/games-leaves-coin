using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new();

    public static void Register<T>(T service)
    {
        services[typeof(T)] = service;
    }

    public static T Get<T>()
    {
        if (services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }

        Debug.LogError($"Service {typeof(T)} not found.");
        return default;
    }

    public static void Clear() => services.Clear();
}
