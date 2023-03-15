using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Stat : ICloneable
{
    public Level level;
    public int Level { get => level.LV; }
    public float maxHP;
    public float atk;
    public float criticalRate;
    public float criticalDmg;
    public float attackSpeed = 1;

    public void LevelUp(Rarity rarity, string id)
    {
        switch (rarity)
        {
            case Rarity.Tier1:
                LevelUpTier1(id);
                break;
            case Rarity.Tier2:
                LevelUpTier2(id);
                break;
            case Rarity.Tier3:
                LevelUpTier3(id);
                break;
            case Rarity.Tier4:
                LevelUpTier4(id);
                break;
        }
    }

    public void LevelUpTier1(string id)
    {
        float tier1AtkRatio = (DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(id).stat.atk / 15) * 3;
        float tier1HPRatio = (DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(id).stat.maxHP / 100) * 20;
        atk += tier1AtkRatio;
        maxHP += tier1HPRatio;
    }

    public void LevelUpTier2(string id)
    {
        float tier2AtkRatio = (DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(id).stat.atk / 20) * 4.5f;
        float tier2HPRatio = (DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(id).stat.maxHP / 200) * 30;
        float ratio = maxHP / atk;
        atk += tier2AtkRatio;
        maxHP += tier2HPRatio;
    }

    public void LevelUpTier3(string id)
    {
        float tier3AtkRatio = (DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(id).stat.atk / 30) * 6;
        float tier3HPRatio = (DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(id).stat.maxHP / 300) * 45;
        atk += tier3AtkRatio;
        maxHP += tier3HPRatio;
    }

    public void LevelUpTier4(string id)
    {
        float tier4AtkRatio = (DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(id).stat.atk / 40) * 9;
        float tier4HPRatio = (DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(id).stat.maxHP / 400) * 70;
        atk += tier4AtkRatio;
        maxHP += tier4HPRatio;
    }

    public Stat CloneStat()
    {
        return (Stat)this.Clone();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
