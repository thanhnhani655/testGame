using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Buff : ICloneable
{
    public string id;
    public string buffName;
    public int buffStack;

    public CombatStatType statType;
    public float buffValue;

    public Buff CloneBuff()
    {
        return (Buff)this.Clone();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    
}
