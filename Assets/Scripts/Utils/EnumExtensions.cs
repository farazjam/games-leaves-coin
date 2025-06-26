using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnumExtensions
{
    /// <summary>
    /// Usage :
    /// int length = EnumExtensions.GetEnumLength<ItemType>(); 
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static int GetEnumLength<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Length;
    }
}
