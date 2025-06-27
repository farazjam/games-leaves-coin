using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CurrencyArg
{
    public CurrencyType Type;
    public long Amount;
    public string Icon;

    public virtual CurrencyArg Clone()
    {
        return new CurrencyArg
        {
            Type = this.Type,
            Amount = this.Amount,
            Icon = this.Icon
        };
    }
}