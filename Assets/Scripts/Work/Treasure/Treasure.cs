using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TreasureType
{
    AllTime,
    AtStartOfInvade,
    KeyUnock
}

[System.Serializable]
public class Treasure :  ICloneable
{
    public string id;
    public string itemName;
    public string desc;
    public Rarity rarity;
    public TreasureType treasureType;
    public int stack = 1;
    public float value1;
    public int maxduplicate = 1;


    public Treasure CloneTreasure()
    {
        return (Treasure)this.Clone();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
