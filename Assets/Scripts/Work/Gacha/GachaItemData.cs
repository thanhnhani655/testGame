using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Rarity
{
    Tier1,
    Tier2,
    Tier3,
    Tier4
}

[Serializable]
public class GachaItemData : ICloneable
{
    public string gachaID;
    public string itemID;
    public string itemName;
    public Rarity itemRarity;
    public MonsterRank rank;
    public int copy = 1;

    public GachaItemData CloneItem()
    {
        return (GachaItemData)(this.Clone());
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
