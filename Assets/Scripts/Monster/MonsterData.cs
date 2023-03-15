using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemAddress
{
    Inventory,
    Dungeon,
    Prison
}

public enum MobFaction
{
    Dungeon,
    Invader
}

public enum MonsterRank
{
    Minion,
    Boss
}

public enum Legion
{
    Red,
    Blue,
    Black
}


[Serializable]
public class MonsterData : ICloneable
{
    public event System.Action OnDead = delegate { };
    public event System.Action OnResetAlive = delegate { };
    public string id;
    public string monName;
    public Rarity rarity;
    public MonsterRank rank;
    public Legion legion;
    public Stat stat;
    public CombatStat baseStat;
    public CombatStat captureStat;
    
    public MobFaction faction;
    public SkillData mainSkill;
    public List<SkillData> subSkill;

    public bool isDead = false;
    public bool isTakeDmg = false;
    public float dmgReceived = 0;

    public ItemAddress address;
    public string dungeonAddress;

    //Corruption
    public bool isCorrupting;
    public bool isCorruptDone;
    public int corruptionTime = 0;
    public bool isCorrupted = false;

    public int bossSkillPoint = 0;
    public float counterToGetSkill = 10;

    public bool LevelUp(bool isGodCommand = false)
    {
        if (!stat.level.LevelUp(isGodCommand))
            return false;

        stat.LevelUp(rarity, id);
        if ((stat.Level % counterToGetSkill) == 0)
        {
            bossSkillPoint++;
        }

        return true;
    }

    public void InscreaseExp(float exp)
    {
        stat.level.InscreaseExp(exp);
        while (true)
        {
            if (!stat.level.IsEnoughExpToLevelUp())
                break;
            this.LevelUp();
        }
    }

    public void SetLevel(int ilevel)
    {
        for (int i = stat.Level; i < ilevel; i++)
        {
            LevelUp(true);
        }
    }

    public void SetLevelUpTo(int ilevel)
    {
        while (true)
        {
            if (this.stat.Level >= ilevel)
                break;
            LevelUp(true);
        }
    }

    public MonsterData CloneMon()
    {
        MonsterData clone = (MonsterData)this.Clone();
        clone.stat = this.stat.CloneStat();
        clone.stat.level = this.stat.level.CloneLevel();
        clone.baseStat = this.baseStat.CloneCombatStat();
        clone.mainSkill = this.mainSkill.CloneSkill();
        return clone;
    }

    public void Dead()
    {
        isDead = true;
        this.OnDead();
    }

    public void ResetAlive()
    {
        isDead = false;
        OnResetAlive();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public void InitializeForInvadeBattle()
    {
        baseStat.Initialize(this.stat);
    }

    public void CaptureStat()
    {
        captureStat = baseStat.Capture();
    }
}
