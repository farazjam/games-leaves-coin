using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CurrencyArgDailyReward : CurrencyArg
{
    public int ResetHourUTC24;
    [HideInInspector] public string LastClaim;
    [HideInInspector] public string NextClaim;

    public override CurrencyArg Clone()
    {
        return new CurrencyArgDailyReward
        {
            Type = this.Type,
            Amount = this.Amount,
            Icon = this.Icon,
            ResetHourUTC24 = this.ResetHourUTC24,
            LastClaim = this.LastClaim,
            NextClaim = this.NextClaim
        };
    }
}