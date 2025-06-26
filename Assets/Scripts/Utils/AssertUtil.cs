using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable CS8632
public static class AssertUtil
{
    public static void IsNotNull(Object _object, string? message = null)
    {
        #if UNITY_EDITOR
        Assert.IsNotNull(_object, message);
        #endif
    }

    public static void IsNotNull(params Object[] _objects)
    {
        #if UNITY_EDITOR
        _objects.ToList().ForEach(obj => Assert.IsNotNull(obj));
        #endif
    }

    public static void IsNotNull<T>(params T[] _objects)
    {
        #if UNITY_EDITOR
        _objects.ToList().ForEach(obj => Debug.Assert(obj != null));
        #endif
    }

    public static void AreEqual<T>(T objectA, T objectB, string? message = null)
    {
        #if UNITY_EDITOR
        Assert.AreEqual(objectA, objectB, message);
        #endif
    }

    public static void AreNotEqual<T>(T objectA, T objectB, string? message = null)
    {
        #if UNITY_EDITOR
        Assert.AreNotEqual(objectA, objectB, message);
        #endif
    }

    public static void IsFalse(bool condition, string? message = null)
    {
        #if UNITY_EDITOR
        Assert.IsFalse(condition, message);
        #endif
    }

    public static void IsTrue(bool condition, string? message = null)
    {
        #if UNITY_EDITOR
        Assert.IsTrue(condition, message);
        #endif
    }
}

#pragma warning restore CS8632
